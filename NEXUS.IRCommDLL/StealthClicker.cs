using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace NEXUS.IRCommDLL;

internal class StealthClicker
{
	private struct POINT
	{
		public int X;

		public int Y;
	}

	public class MousePattern
	{
		public List<System.Windows.Point> Points { get; set; }

		public List<int> Timings { get; set; }

		public DateTime Created { get; set; }
	}

	public enum ClickMethod
	{
		JavaScript,
		ActionsAPI,
		Hybrid,
		Direct,
		WindowsAPI
	}

	private static Random _rnd = new Random();

	private static List<MousePattern> _mousePatterns = new List<MousePattern>();

	private const uint MOUSEEVENTF_MOVE = 1u;

	private const uint MOUSEEVENTF_LEFTDOWN = 2u;

	private const uint MOUSEEVENTF_LEFTUP = 4u;

	private const uint MOUSEEVENTF_ABSOLUTE = 32768u;

	[DllImport("user32.dll")]
	private static extern bool GetCursorPos(out POINT lpPoint);

	[DllImport("user32.dll")]
	private static extern bool SetCursorPos(int X, int Y);

	[DllImport("user32.dll")]
	private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

	internal void HumanClick(IWebDriver driver, By locator, ClickMethod preferredMethod = ClickMethod.Hybrid)
	{
		try
		{
			ExecutePreClickBehavior(driver);
			IWebElement webElement = FindElementHumanLike(driver, locator);
			if (webElement != null)
			{
				EnsureElementReady(driver, webElement);
				ClickMethod method = SelectClickMethod(preferredMethod, driver, webElement);
				ExecuteClick(driver, webElement, method);
				ExecutePostClickBehavior(driver);
			}
		}
		catch (Exception ex)
		{
			HandleErrorHumanLike(driver, ex);
		}
	}

	private static void ExecutePreClickBehavior(IWebDriver driver)
	{
		Thread.Sleep(_rnd.Next(100, 800));
		if (_rnd.NextDouble() < 0.3)
		{
			ExecuteRandomMicroMovements(driver);
		}
		if (_rnd.NextDouble() < 0.1)
		{
			ExecuteRandomScroll(driver);
		}
	}

	private static IWebElement FindElementHumanLike(IWebDriver driver, By locator)
	{
		IWebElement webElement = null;
		int num = 2 + _rnd.Next(3);
		for (int i = 0; i < num; i++)
		{
			try
			{
				if (_rnd.NextDouble() < 0.6)
				{
					webElement = new WebDriverWait(driver, TimeSpan.FromSeconds(3.0)).Until(delegate(IWebDriver drv)
					{
						IWebElement webElement2 = drv.FindElement(locator);
						return (!webElement2.Displayed || !webElement2.Enabled) ? null : webElement2;
					});
				}
				else
				{
					webElement = driver.FindElement(locator);
					if (!webElement.Displayed || !webElement.Enabled)
					{
						webElement = null;
					}
				}
				if (webElement != null)
				{
					break;
				}
			}
			catch
			{
				Thread.Sleep(200 + _rnd.Next(600));
			}
			if (i < num - 1)
			{
				Thread.Sleep(300 + _rnd.Next(700));
			}
		}
		return webElement;
	}

	private static void EnsureElementReady(IWebDriver driver, IWebElement element)
	{
		try
		{
			IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
			string script = "var rect = arguments[0].getBoundingClientRect();\r\n                return (\r\n                    rect.top >= 0 &&\r\n                    rect.left >= 0 &&\r\n                    rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&\r\n                    rect.right <= (window.innerWidth || document.documentElement.clientWidth)\r\n                );";
			if (!(bool)javaScriptExecutor.ExecuteScript(script, element))
			{
				ScrollToElementHumanLike(driver, element);
			}
		}
		catch
		{
		}
		if (_rnd.NextDouble() < 0.7)
		{
			HoverOverElement(driver, element);
		}
	}

	private static ClickMethod SelectClickMethod(ClickMethod preferred, IWebDriver driver, IWebElement element)
	{
		List<ClickMethod> list = new List<ClickMethod>();
		for (int i = 0; i < 3; i++)
		{
			list.Add(preferred);
		}
		if (IsModernBrowser(driver))
		{
			list.Add(ClickMethod.JavaScript);
		}
		if (IsComplexElement(element))
		{
			list.Add(ClickMethod.ActionsAPI);
		}
		list.Add(ClickMethod.Hybrid);
		list.Add(ClickMethod.Direct);
		if (_rnd.NextDouble() < 0.1)
		{
			list.Add(ClickMethod.WindowsAPI);
		}
		return list[_rnd.Next(list.Count)];
	}

	private static void ExecuteClick(IWebDriver driver, IWebElement element, ClickMethod method)
	{
		switch (method)
		{
		case ClickMethod.JavaScript:
			JavaScriptClick(driver, element);
			break;
		case ClickMethod.ActionsAPI:
			ActionsAPIClick(driver, element);
			break;
		case ClickMethod.Hybrid:
			HybridClick(driver, element);
			break;
		case ClickMethod.Direct:
			DirectClick(element);
			break;
		case ClickMethod.WindowsAPI:
			WindowsAPIClick(driver, element);
			break;
		}
	}

	private static void JavaScriptClick(IWebDriver driver, IWebElement element)
	{
		string text = "\r\n(function(el) {\r\n    // Avoid detection by not creating global variables\r\n    var clickSimulation = {\r\n        execute: function() {\r\n            var rect = el.getBoundingClientRect();\r\n            var centerX = rect.left + rect.width/2;\r\n            var centerY = rect.top + rect.height/2;\r\n            \r\n            // Human-like offset\r\n            var offsetX = (Math.random() - 0.5) * Math.min(15, rect.width * 0.3);\r\n            var offsetY = (Math.random() - 0.5) * Math.min(12, rect.height * 0.25);\r\n            \r\n            var targetX = centerX + offsetX;\r\n            var targetY = centerY + offsetY;\r\n            \r\n            // Generate event sequence\r\n            var events = [];\r\n            \r\n            // Mouse movement with pauses\r\n            var moveSteps = 2 + Math.floor(Math.random() * 3);\r\n            for(var i = 0; i < moveSteps; i++) {\r\n                events.push({\r\n                    type: 'mousemove',\r\n                    x: targetX - (moveSteps - i) * 10 + Math.random() * 8,\r\n                    y: targetY - (moveSteps - i) * 8 + Math.random() * 6,\r\n                    delay: i * (80 + Math.random() * 60)\r\n                });\r\n            }\r\n            \r\n            // Click events\r\n            var clickDelay = events[events.length-1].delay + 50;\r\n            ['mouseover', 'mousedown', 'mouseup', 'click'].forEach(function(type, idx) {\r\n                events.push({\r\n                    type: type,\r\n                    x: targetX + (Math.random() - 0.5) * 3,\r\n                    y: targetY + (Math.random() - 0.5) * 2,\r\n                    delay: clickDelay + idx * (30 + Math.random() * 30)\r\n                });\r\n            });\r\n            \r\n            // Execute events\r\n            events.forEach(function(evt) {\r\n                setTimeout(function() {\r\n                    var mouseEvt = new MouseEvent(evt.type, {\r\n                        bubbles: true,\r\n                        cancelable: true,\r\n                        clientX: evt.x,\r\n                        clientY: evt.y,\r\n                        view: window,\r\n                        button: 0,\r\n                        buttons: evt.type === 'mousedown' ? 1 : 0\r\n                    });\r\n                    \r\n                    // Dispatch with natural target\r\n                    var target = document.elementFromPoint(evt.x, evt.y);\r\n                    if (target) {\r\n                        target.dispatchEvent(mouseEvt);\r\n                    }\r\n                    \r\n                    // Also dispatch on element for reliability\r\n                    if (target !== el) {\r\n                        el.dispatchEvent(new MouseEvent(evt.type, {\r\n                            bubbles: true,\r\n                            clientX: evt.x,\r\n                            clientY: evt.y,\r\n                            view: window\r\n                        }));\r\n                    }\r\n                }, evt.delay);\r\n            });\r\n            \r\n            // Fallback click\r\n            setTimeout(function() {\r\n                try {\r\n                    el.click();\r\n                } catch(e) {\r\n                    // Silent fallback\r\n                }\r\n            }, clickDelay + 200 + Math.random() * 100);\r\n        }\r\n    };\r\n    \r\n    // Execute with random delay\r\n    setTimeout(function() {\r\n        clickSimulation.execute();\r\n    }, Math.random() * 50);\r\n    \r\n})(arguments[0]);";
		string script = text;
		((IJavaScriptExecutor)driver).ExecuteScript(script, element);
		Thread.Sleep(_rnd.Next(300, 1000));
	}

	private static void ActionsAPIClick(IWebDriver driver, IWebElement element)
	{
		Actions actions = new Actions(driver);
		int num = _rnd.Next(-(int)((double)element.Size.Width * 0.25), (int)((double)element.Size.Width * 0.25));
		int num2 = _rnd.Next(-(int)((double)element.Size.Height * 0.25), (int)((double)element.Size.Height * 0.25));
		actions.MoveToElement(element, num / 3, num2 / 3).Pause(TimeSpan.FromMilliseconds(_rnd.Next(80, 200))).MoveToElement(element, num * 2 / 3, num2 * 2 / 3)
			.Pause(TimeSpan.FromMilliseconds(_rnd.Next(50, 150)))
			.MoveToElement(element, num, num2)
			.Pause(TimeSpan.FromMilliseconds(_rnd.Next(100, 300)))
			.Click()
			.Pause(TimeSpan.FromMilliseconds(_rnd.Next(50, 150)))
			.Perform();
	}

	private static void HybridClick(IWebDriver driver, IWebElement element)
	{
		new Actions(driver).MoveToElement(element).Perform();
		Thread.Sleep(_rnd.Next(150, 400));
		string text = "\r\n(function(el) {\r\n    // Simple trusted click\r\n    var evt = new MouseEvent('click', {\r\n        bubbles: true,\r\n        cancelable: true,\r\n        view: window\r\n    });\r\n    el.dispatchEvent(evt);\r\n})(arguments[0]);";
		string script = text;
		((IJavaScriptExecutor)driver).ExecuteScript(script, element);
		IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
		string script2 = "\r\n(function(el) {\r\n    ['mousedown', 'mouseup'].forEach(function(type) {\r\n        var evt = new MouseEvent(type, {bubbles: true, view: window});\r\n        el.dispatchEvent(evt);\r\n    });\r\n})(arguments[0])";
		javaScriptExecutor.ExecuteScript(script2, element);
		Thread.Sleep(_rnd.Next(200, 600));
	}

	private static void DirectClick(IWebElement element)
	{
		Thread.Sleep(_rnd.Next(200, 500));
		element.Click();
		Thread.Sleep(_rnd.Next(300, 700));
	}

	private static void WindowsAPIClick(IWebDriver driver, IWebElement element)
	{
		try
		{
			System.Drawing.Point location = element.Location;
			System.Drawing.Size size = element.Size;
			int num = location.X + size.Width / 2 + 100;
			int num2 = location.Y + size.Height / 2 + 100;
			num += _rnd.Next(-8, 9);
			num2 += _rnd.Next(-6, 7);
			GetCursorPos(out var lpPoint);
			SimulateHumanMovement(lpPoint.X, lpPoint.Y, num, num2);
			Thread.Sleep(_rnd.Next(80, 200));
			mouse_event(2u, (uint)num, (uint)num2, 0u, 0);
			Thread.Sleep(100 + _rnd.Next(150));
			mouse_event(4u, (uint)num, (uint)num2, 0u, 0);
		}
		catch
		{
			DirectClick(element);
		}
	}

	private static void SimulateHumanMovement(int startX, int startY, int endX, int endY)
	{
		int num = 15 + _rnd.Next(10);
		for (int i = 0; i <= num; i++)
		{
			double num2 = (double)i / (double)num;
			double num3 = (double)startX + (double)(endX - startX) * 0.3 + (double)_rnd.Next(-10, 11);
			double num4 = (double)startY + (double)(endY - startY) * 0.4 + (double)_rnd.Next(-8, 9);
			double num5 = (double)startX + (double)(endX - startX) * 0.7 + (double)_rnd.Next(-8, 9);
			double num6 = (double)startY + (double)(endY - startY) * 0.6 + (double)_rnd.Next(-6, 7);
			double num7 = 1.0 - num2;
			double num8 = num2 * num2;
			double num9 = num7 * num7;
			double num10 = num9 * num7;
			double num11 = num8 * num2;
			double num12 = num10 * (double)startX + 3.0 * num9 * num2 * num3 + 3.0 * num7 * num8 * num5 + num11 * (double)endX;
			double num13 = num10 * (double)startY + 3.0 * num9 * num2 * num4 + 3.0 * num7 * num8 * num6 + num11 * (double)endY;
			num12 += _rnd.NextDouble() * 3.0 - 1.5;
			num13 += _rnd.NextDouble() * 2.0 - 1.0;
			SetCursorPos((int)num12, (int)num13);
			Thread.Sleep(_rnd.Next(10, 25));
		}
	}

	private static void ExecuteRandomMicroMovements(IWebDriver driver)
	{
		string text = "\r\n// Natural idle movements\r\nvar movements = 1 + Math.floor(Math.random() * 3);\r\nfor(var i = 0; i < movements; i++) {\r\n    setTimeout(function() {\r\n        var x = window.innerWidth * (0.4 + Math.random() * 0.2);\r\n        var y = window.innerHeight * (0.3 + Math.random() * 0.4);\r\n        document.dispatchEvent(new MouseEvent('mousemove', {\r\n            clientX: x,\r\n            clientY: y,\r\n            bubbles: true\r\n        }));\r\n    }, i * (100 + Math.random() * 200));\r\n}";
		string script = text;
		try
		{
			((IJavaScriptExecutor)driver).ExecuteScript(script);
		}
		catch
		{
		}
	}

	private static void ScrollToElementHumanLike(IWebDriver driver, IWebElement element)
	{
		string text = "\r\n(function(el) {\r\n    // Smooth scroll with variations\r\n    var behavior = Math.random() < 0.7 ? 'smooth' : 'auto';\r\n    var block = ['center', 'start', 'end', 'nearest'][Math.floor(Math.random() * 4)];\r\n    el.scrollIntoView({behavior: behavior, block: block, inline: 'nearest'});\r\n})(arguments[0])";
		string script = text;
		((IJavaScriptExecutor)driver).ExecuteScript(script, element);
		Thread.Sleep(_rnd.Next(400, 1000));
	}

	private static void HoverOverElement(IWebDriver driver, IWebElement element)
	{
		string text = "\r\n(function(el) {\r\n    // Natural hover with slight movement\r\n    var rect = el.getBoundingClientRect();\r\n    var points = [\r\n        {x: rect.left + rect.width * 0.3, y: rect.top + rect.height * 0.4},\r\n        {x: rect.left + rect.width * 0.6, y: rect.top + rect.height * 0.5},\r\n        {x: rect.left + rect.width * 0.5, y: rect.top + rect.height * 0.6}\r\n    ];\r\n    \r\n    points.forEach(function(point, i) {\r\n        setTimeout(function() {\r\n            var evt = new MouseEvent('mousemove', {\r\n                clientX: point.x,\r\n                clientY: point.y,\r\n                bubbles: true\r\n            });\r\n            document.dispatchEvent(evt);\r\n        }, i * (100 + Math.random() * 150));\r\n    });\r\n})(arguments[0])";
		string script = text;
		((IJavaScriptExecutor)driver).ExecuteScript(script, element);
		Thread.Sleep(_rnd.Next(300, 600));
	}

	private static void ExecuteRandomScroll(IWebDriver driver)
	{
		string text = "\r\n// Random small scroll\r\nvar scrollY = (Math.random() - 0.5) * 200;\r\nwindow.scrollBy({top: scrollY, behavior: 'smooth'});";
		string script = text;
		((IJavaScriptExecutor)driver).ExecuteScript(script);
		Thread.Sleep(_rnd.Next(300, 800));
	}

	private static void ExecutePostClickBehavior(IWebDriver driver)
	{
		Thread.Sleep(_rnd.Next(200, 1200));
		if (_rnd.NextDouble() < 0.5)
		{
			string text = "\r\n// Move to random position\r\nsetTimeout(function() {\r\n    var x = window.innerWidth * (0.2 + Math.random() * 0.6);\r\n    var y = window.innerHeight * (0.2 + Math.random() * 0.6);\r\n    document.dispatchEvent(new MouseEvent('mousemove', {\r\n        clientX: x,\r\n        clientY: y,\r\n        bubbles: true\r\n    }));\r\n}, ";
			string text2 = _rnd.Next(300, 1500).ToString();
			string text3 = ")";
			string script = text + text2 + text3;
			((IJavaScriptExecutor)driver).ExecuteScript(script);
		}
	}

	private static void HandleErrorHumanLike(IWebDriver driver, Exception ex)
	{
		Thread.Sleep(_rnd.Next(500, 1500));
		if (!(_rnd.NextDouble() < 0.3))
		{
			return;
		}
		try
		{
			if (_rnd.NextDouble() < 0.5)
			{
				driver.Navigate().Back();
			}
			else
			{
				driver.Navigate().Refresh();
			}
			Thread.Sleep(_rnd.Next(1000, 3000));
		}
		catch
		{
		}
	}

	private static bool IsModernBrowser(IWebDriver driver)
	{
		try
		{
			_ = ((IHasCapabilities)driver).Capabilities;
			return true;
		}
		catch
		{
			return false;
		}
	}

	private static bool IsComplexElement(IWebElement element)
	{
		try
		{
			string attributeName = "onclick";
			int num;
			if (element.GetAttribute(attributeName) == null)
			{
				string attributeName2 = "data-action";
				if (element.GetAttribute(attributeName2) == null)
				{
					string tagName = element.TagName;
					string text = "button";
					if (!(tagName == text))
					{
						string tagName2 = element.TagName;
						string text2 = "a";
						num = ((tagName2 == text2) ? 1 : 0);
						goto IL_0065;
					}
				}
			}
			num = 1;
			goto IL_0065;
			IL_0065:
			return (byte)num != 0;
		}
		catch
		{
			return false;
		}
	}
}
