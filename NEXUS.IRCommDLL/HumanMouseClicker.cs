using Microsoft.Playwright;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SeleniumUndetectedChromeDriver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Keys = OpenQA.Selenium.Keys;

namespace NEXUS.IRCommDLL;

internal class HumanMouseClicker
{
	private struct POINT
	{
		public int X;

		public int Y;
	}

	private const int MOUSEEVENTF_LEFTDOWN = 2;

	private const int MOUSEEVENTF_LEFTUP = 4;

	private static readonly ThreadLocal<Random> _random = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
    private bool _injected;
    private const int KEYEVENTF_KEYUP = 2;

	private static Random rnd => _random.Value;

	[DllImport("user32.dll")]
	private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int extraInfo);

	[DllImport("user32.dll")]
	private static extern bool SetCursorPos(int X, int Y);

	[DllImport("user32.dll")]
	private static extern bool GetCursorPos(out POINT lpPoint);

    internal void HumanLikeClickWithRandomApproach(IWebDriver driver, By locator)
    {
        try
        {
            IWebElement webElement = driver.FindElement(locator);
            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
            POINT accurateScreenCoordinates = GetAccurateScreenCoordinates(driver, webElement);
            int x = accurateScreenCoordinates.X;
            int y = accurateScreenCoordinates.Y;
            GetCursorPos(out var lpPoint);
            VerifyCoordinates(driver, webElement, x, y);
            HumanMoveToPosition(lpPoint.X, lpPoint.Y, x, y);
            GetCursorPos(out var _);
            Thread.Sleep(rnd.Next(100, 250));
            if (IsElementUnderCursor(driver, webElement, x, y))
            {
                mouse_event(2, x, y, 0, 0);
                Thread.Sleep(rnd.Next(30, 100));
                mouse_event(4, x, y, 0, 0);
            }
            else
            {
                string script = "arguments[0].click();";
                javaScriptExecutor.ExecuteScript(script, webElement);
            }
        }
        catch (Exception)
        {
            IWebElement webElement2 = driver.FindElement(locator);
            IJavaScriptExecutor javaScriptExecutor2 = (IJavaScriptExecutor)driver;
            string script2 = "arguments[0].click();";
            javaScriptExecutor2.ExecuteScript(script2, webElement2);
        }
    }
    internal async Task HumanLikeClickWithRandomApproachEDGE(IPage page, string selector)
    {
        try
        {
            var element = await page.QuerySelectorAsync(selector);
            if (element == null)
                throw new Exception("Element not found");

            var box = await element.BoundingBoxAsync();
            if (box == null)
                throw new Exception("Element not visible");

            // Target position (center of element)
            float targetX = box.X + box.Width / 2;
            float targetY = box.Y + box.Height / 2;

            // Random start position
            var rnd = new Random();
            float startX = targetX - rnd.Next(100, 300);
            float startY = targetY - rnd.Next(100, 300);

            // Move mouse in steps (human-like)
            await page.Mouse.MoveAsync(startX, startY);
            await page.Mouse.MoveAsync(targetX, targetY, new() { Steps = rnd.Next(10, 25) });

            await Task.Delay(rnd.Next(100, 250));

            // Click
            await page.Mouse.DownAsync();
            await Task.Delay(rnd.Next(30, 100));
            await page.Mouse.UpAsync();
        }
        catch
        {
            // Fallback click
            await page.ClickAsync(selector);
        }
    }
    internal void RealHumanClick(IWebDriver driver, By locator)
    {
        IWebElement element = driver.FindElement(locator);
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        Random rnd = new Random();

        js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
        Thread.Sleep(rnd.Next(300, 600));

        // 1. Element position in viewport
        var rect = (ReadOnlyCollection<object>)js.ExecuteScript(@"
        var r = arguments[0].getBoundingClientRect();
        return [r.left + r.width/2, r.top + r.height/2];
    ", element);

        double x = Convert.ToDouble(rect[0]);
        double y = Convert.ToDouble(rect[1]);

        // 2. Window + chrome offset correction
        var windowData = (ReadOnlyCollection<object>)js.ExecuteScript(@"
        return [
            window.screenX,
            window.screenY,
            window.outerHeight - window.innerHeight
        ];
    ");

        int screenX = Convert.ToInt32(windowData[0]);
        int screenY = Convert.ToInt32(windowData[1]);
        int chromeOffset = Convert.ToInt32(windowData[2]);

        int offsetX = rnd.Next(-30, 30);
        int offsetY = rnd.Next(-4, 5);

        int targetX = screenX + (int)x + offsetX;
        int targetY = screenY + chromeOffset + (int)y + offsetY;

        // 4. Move human-like
        GetCursorPos(out var start);
        HumanMoveToPosition(start.X, start.Y, targetX, targetY);

        Thread.Sleep(rnd.Next(120, 300));

        // 5. Click
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        Thread.Sleep(rnd.Next(60, 140));
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
    }

    internal void RealHumanClickEDGE(IWebDriver driver, By locator)
    {
        IWebElement element = driver.FindElement(locator);
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        Random rnd = new Random();

        // Scroll into view
        js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
        Thread.Sleep(rnd.Next(300, 600));

        // Get element center (viewport)
        var rect = (ReadOnlyCollection<object>)js.ExecuteScript(@"
        var r = arguments[0].getBoundingClientRect();
        return [r.left + r.width/2, r.top + r.height/2];
    ", element);

        double x = Convert.ToDouble(rect[0]);
        double y = Convert.ToDouble(rect[1]);

        // Get browser window position (more reliable)
        var windowPos = driver.Manage().Window.Position;

        int offsetX = rnd.Next(-20, 20);
        int offsetY = rnd.Next(-5, 5);

        int targetX = windowPos.X + (int)x + offsetX;
        int targetY = windowPos.Y + (int)y + offsetY + 85; // approx title+tabs

        // Move mouse
        GetCursorPos(out var start);
        HumanMoveToPosition(start.X, start.Y, targetX, targetY);

        Thread.Sleep(rnd.Next(120, 300));

        // Click (better approach)
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        Thread.Sleep(rnd.Next(60, 140));
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
    }
    internal void HumanLikeClickWithRandomApproach_A(IWebDriver driver, By locator)
    {
        try
        {
            IWebElement element = driver.FindElement(locator);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            // 1. Scroll to center
            js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
            Thread.Sleep(rnd.Next(400, 700));

            // 2. Get browser + viewport corrected coordinates
            var rect = (ReadOnlyCollection<object>)js.ExecuteScript(@"
            var r = arguments[0].getBoundingClientRect();
            return [r.left + r.width/2, r.top + r.height/2];
        ", element);

            double elementX = Convert.ToDouble(rect[0]);
            double elementY = Convert.ToDouble(rect[1]);

            // 3. Get window offsets (important fix for bottom-screen issue)
            var windowData = (ReadOnlyCollection<object>)js.ExecuteScript(@"
            return [
                window.screenX,
                window.screenY,
                window.outerHeight - window.innerHeight
            ];
        ");

            int screenX = Convert.ToInt32(windowData[0]);
            int screenY = Convert.ToInt32(windowData[1]);
            int chromeOffset = Convert.ToInt32(windowData[2]);

            // 4. Final corrected coordinates
            int targetX = screenX + (int)Math.Round(elementX);
            int targetY = screenY + chromeOffset + (int)Math.Round(elementY);

            // 5. Current cursor position
            GetCursorPos(out var currentPos);

            // 6. Human movement
            HumanMoveToPosition(currentPos.X, currentPos.Y, targetX, targetY);

            Thread.Sleep(rnd.Next(120, 260));

            // 7. Click check
            if (IsCursorNear(targetX, targetY))
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                Thread.Sleep(rnd.Next(40, 120));
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
            else
            {
                js.ExecuteScript("arguments[0].click();", element);
            }
        }
        catch
        {
            try
            {
                IWebElement element = driver.FindElement(locator);
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].click();", element);
            }
            catch { }
        }
    }
    private bool IsCursorNear(int x, int y, int tolerance = 10)
    {
        GetCursorPos(out var p);
        return Math.Abs(p.X - x) <= tolerance &&
               Math.Abs(p.Y - y) <= tolerance;
    }
    internal void HumanLikeClickWithRandomApproach_Play(IPage page, By locator)
	{
		try
		{
			string text = locator.ToString();
			string oldValue = "By.CssSelector: ";
			string text2 = text.Replace(oldValue, "");
			IElementHandle result = page.QuerySelectorAsync(text2).GetAwaiter().GetResult();
			if (result == null)
			{
				string text3 = "Element not found: ";
				Console.WriteLine(text3 + text2);
				return;
			}
			POINT accurateScreenCoordinates_Play = GetAccurateScreenCoordinates_Play(page, result);
			int x = accurateScreenCoordinates_Play.X;
			int y = accurateScreenCoordinates_Play.Y;
			GetCursorPos(out var lpPoint);
			VerifyCoordinates_Play(page, result, x, y);
			HumanMoveToPosition(lpPoint.X, lpPoint.Y, x, y);
			GetCursorPos(out var _);
			Thread.Sleep(rnd.Next(100, 250));
			if (IsElementUnderCursor_Play(page, result, x, y))
			{
				mouse_event(2, x, y, 0, 0);
				Thread.Sleep(rnd.Next(30, 100));
				mouse_event(4, x, y, 0, 0);
			}
			else
			{
				string expression = "el => el.click()";
				page.EvaluateAsync(expression, result).GetAwaiter().GetResult();
			}
		}
		catch
		{
			try
			{
				string text4 = locator.ToString();
				string oldValue2 = "By.CssSelector: ";
				string selector = text4.Replace(oldValue2, "");
				IElementHandle result2 = page.QuerySelectorAsync(selector).GetAwaiter().GetResult();
				if (result2 != null)
				{
					string expression2 = "el => el.click()";
					page.EvaluateAsync(expression2, result2).GetAwaiter().GetResult();
				}
			}
			catch
			{
			}
		}
	}

	[DllImport("user32.dll")]
	public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

	internal void HumanLikeClickAndPaste(IWebDriver driver, By locator, string text)
	{
		try
		{
			IWebElement webElement = driver.FindElement(locator);
			IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
			POINT accurateScreenCoordinates = GetAccurateScreenCoordinates(driver, webElement);
			int x = accurateScreenCoordinates.X;
			int y = accurateScreenCoordinates.Y;
			GetCursorPos(out var lpPoint);
			VerifyCoordinates(driver, webElement, x, y);
			HumanMoveToPosition(lpPoint.X, lpPoint.Y, x, y);
			GetCursorPos(out var _);
			Thread.Sleep(rnd.Next(100, 250));
			if (IsElementUnderCursor(driver, webElement, x, y))
			{
				mouse_event(2, x, y, 0, 0);
				Thread.Sleep(rnd.Next(30, 100));
				mouse_event(4, x, y, 0, 0);
				Thread.Sleep(rnd.Next(120, 220));
				Clipboard.SetText(text);
				Thread.Sleep(rnd.Next(120, 200));
				keybd_event(17, 0, 0, 0);
				keybd_event(86, 0, 0, 0);
				Thread.Sleep(50);
				keybd_event(86, 0, 2, 0);
				keybd_event(17, 0, 2, 0);
			}
			else
			{
				string script = "arguments[0].click();";
				javaScriptExecutor.ExecuteScript(script, webElement);
				Clipboard.SetText(text);
				Thread.Sleep(150);
				string keys = "^v";
				SendKeys.SendWait(keys);
			}
		}
		catch (Exception)
		{
			IWebElement webElement2 = driver.FindElement(locator);
			IJavaScriptExecutor javaScriptExecutor2 = (IJavaScriptExecutor)driver;
			string script2 = "arguments[0].click();";
			javaScriptExecutor2.ExecuteScript(script2, webElement2);
			Clipboard.SetText(text);
			Thread.Sleep(150);
			string keys2 = "^v";
			SendKeys.SendWait(keys2);
		}
	}
    internal void HumanLikeClick(IWebDriver driver, By locator)
    {
        try
        {
            IWebElement webElement = driver.FindElement(locator);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            // Scroll to center (important)
            js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", webElement);
            Thread.Sleep(rnd.Next(200, 400));

            // Get accurate screen coordinates
            POINT point = GetAccurateScreenCoordinates(driver, webElement);
            int x = point.X;
            int y = point.Y;

            // Current cursor position
            GetCursorPos(out var currentPos);

            // Optional validation
            VerifyCoordinates(driver, webElement, x, y);

            // Human-like movement
            HumanMoveToPosition(currentPos.X, currentPos.Y, x, y);

            Thread.Sleep(rnd.Next(100, 250));

            // Check cursor is on element
            if (IsElementUnderCursor(driver, webElement, x, y))
            {
                // Mouse down
                mouse_event(2, x, y, 0, 0);
                Thread.Sleep(rnd.Next(50, 120));

                // Mouse up
                mouse_event(4, x, y, 0, 0);
            }
            else
            {
                // Fallback JS click
                js.ExecuteScript("arguments[0].click();", webElement);
            }
        }
        catch (Exception)
        {
            // अंतिम fallback
            IWebElement element = driver.FindElement(locator);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
        }
    }
    internal void HumanLikeClickAndPaste_Play(IPage page, By locator, string text)
	{
		try
		{
			string text2 = locator.ToString();
			string oldValue = "By.CssSelector: ";
			string text3 = text2.Replace(oldValue, "");
			IElementHandle result = page.QuerySelectorAsync(text3).GetAwaiter().GetResult();
			if (result == null)
			{
				string text4 = "Element not found: ";
				Console.WriteLine(text4 + text3);
				return;
			}
			POINT accurateScreenCoordinates_Play = GetAccurateScreenCoordinates_Play(page, result);
			int x = accurateScreenCoordinates_Play.X;
			int y = accurateScreenCoordinates_Play.Y;
			GetCursorPos(out var lpPoint);
			HumanMoveToPosition(lpPoint.X, lpPoint.Y, x, y);
			GetCursorPos(out var _);
			Thread.Sleep(rnd.Next(100, 250));
			if (IsElementUnderCursor_Play(page, result, x, y))
			{
				mouse_event(2, x, y, 0, 0);
				Thread.Sleep(rnd.Next(30, 100));
				mouse_event(4, x, y, 0, 0);
				Thread.Sleep(rnd.Next(120, 220));
				Clipboard.SetText(text);
				Thread.Sleep(rnd.Next(120, 200));
				keybd_event(17, 0, 0, 0);
				keybd_event(86, 0, 0, 0);
				Thread.Sleep(50);
				keybd_event(86, 0, 2, 0);
				keybd_event(17, 0, 2, 0);
			}
			else
			{
				string expression = "el => el.click()";
				page.EvaluateAsync(expression, result).GetAwaiter().GetResult();
				Clipboard.SetText(text);
				Thread.Sleep(150);
				string keys = "^v";
				SendKeys.SendWait(keys);
			}
		}
		catch
		{
			try
			{
				string text5 = locator.ToString();
				string oldValue2 = "By.CssSelector: ";
				string selector = text5.Replace(oldValue2, "");
				IElementHandle result2 = page.QuerySelectorAsync(selector).GetAwaiter().GetResult();
				if (result2 != null)
				{
					string expression2 = "el => el.click()";
					page.EvaluateAsync(expression2, result2).GetAwaiter().GetResult();
					Clipboard.SetText(text);
					Thread.Sleep(150);
					string keys2 = "^v";
					SendKeys.SendWait(keys2);
				}
			}
			catch
			{
			}
		}
	}
    internal void TypeLikeHuman(IWebDriver driver, By by, string text, int timeoutSeconds = 40)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
        var element = wait.Until(ExpectedConditions.ElementToBeClickable(by));
        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
        Thread.Sleep(rnd.Next(200, 500));
        element.Click();
        Thread.Sleep(rnd.Next(150, 300));
        element.Clear();
        Thread.Sleep(rnd.Next(150, 300));
        foreach (char c in text)
        {
            element.SendKeys(c.ToString());
            Thread.Sleep(rnd.Next(80, 180));
        }
    }
    internal void method_33(UndetectedChromeDriver undetectedChromeDriver_1, By by_0, string string_7, int int_0)
    {
        Random random = new Random();
        IWebElement webElement = new WebDriverWait(undetectedChromeDriver_1, TimeSpan.FromSeconds(int_0)).Until(ExpectedConditions.ElementIsVisible(by_0));
        ((IJavaScriptExecutor)undetectedChromeDriver_1).ExecuteScript("\r\n            arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});\r\n        ", new object[1] { webElement });
        Thread.Sleep(random.Next(500, 900));
        ((IJavaScriptExecutor)undetectedChromeDriver_1).ExecuteScript("\r\n            var el = arguments[0];\r\n            var rect = el.getBoundingClientRect();\r\n            var startX = Math.floor(Math.random() * window.innerWidth); \r\n            var startY = Math.floor(Math.random() * window.innerHeight); \r\n            var endX = rect.left + rect.width / 2;\r\n            var endY = rect.top + rect.height / 2;\r\n\r\n            function fire(type, x, y){\r\n                el.dispatchEvent(new MouseEvent(type,{\r\n                    bubbles:true,\r\n                    cancelable:true,\r\n                    view:window,\r\n                    clientX:x, clientY:y,\r\n                    screenX:window.screenX+x,\r\n                    screenY:window.screenY+y,\r\n                    button:0\r\n                }));\r\n            }\r\n\r\n            var steps = 20;\r\n            var cx = (startX + endX) / 2 + (Math.random()*50 - 25);\r\n            var cy = (startY + endY) / 2 + (Math.random()*50 - 25);\r\n\r\n            for (var i = 0; i <= steps; i++) {\r\n                var t = i / steps;\r\n                var x = Math.pow(1-t, 2) * startX + 2*(1-t)*t*cx + Math.pow(t,2)*endX;\r\n                var y = Math.pow(1-t, 2) * startY + 2*(1-t)*t*cy + Math.pow(t,2)*endY;\r\n                fire('mousemove', x, y);\r\n            }\r\n\r\n            fire('mousedown', endX, endY);\r\n            fire('mouseup', endX, endY);\r\n            fire('click', endX, endY);\r\n        ", new object[1] { webElement });
        Thread.Sleep(random.Next(200, 400));
        webElement.SendKeys(OpenQA.Selenium.Keys.Control + "a");
        Thread.Sleep(random.Next(100, 250));
        webElement.SendKeys(OpenQA.Selenium.Keys.Backspace);
        Thread.Sleep(random.Next(200, 350));
        Clipboard.SetText(string_7);
        webElement.SendKeys(OpenQA.Selenium.Keys.Control + "v");
    }
    internal void SetTextHuman(
        UndetectedChromeDriver driver,
        By locator,
        string value,
        int timeout = 20)
    {
        IWebElement element = new WebDriverWait(driver,
            TimeSpan.FromSeconds(timeout))
            .Until(ExpectedConditions.ElementToBeClickable(locator));

        ((IJavaScriptExecutor)driver).ExecuteScript(
            "arguments[0].scrollIntoView({block:'center'});",
            element);

        Thread.Sleep(300);

        element.Click();

        Thread.Sleep(300);

        element.SendKeys(Keys.Control + "a");
        element.SendKeys(Keys.Delete);

        Thread.Sleep(300);

        element.SendKeys(value);

        Thread.Sleep(500);

        // PrimeNG / Angular update
        ((IJavaScriptExecutor)driver).ExecuteScript(@"
        arguments[0].dispatchEvent(new Event('input', { bubbles: true }));
        arguments[0].dispatchEvent(new Event('change', { bubbles: true }));
        arguments[0].dispatchEvent(new Event('blur', { bubbles: true }));
    ", element);

        Thread.Sleep(500);

        element.SendKeys(Keys.Enter);

        Thread.Sleep(500);
    }
    private void InjectIfNeeded(UndetectedChromeDriver driver)
    {
        if (!_injected)
        {
            string script = "\r\n(function(){\r\n    if (window.__humanMouse && window.__humanMouse.humanMoveAndLeftClick) return;\r\n    let lastMouse = { x: window.innerWidth/2, y: window.innerHeight/2 };\r\n    document.addEventListener('mousemove', e => { lastMouse.x = e.clientX; lastMouse.y = e.clientY; }, { passive:true });\r\n\r\n    function rand(min,max){ return Math.floor(Math.random()*(max-min+1))+min; }\r\n    function sleep(ms){ return new Promise(res=>setTimeout(res,ms)); }\r\n\r\n    function dispatchMouseEvent(el, type, clientX, clientY, button = 0) {\r\n        const ev = new MouseEvent(type, {\r\n            bubbles: true, cancelable: true, view: window,\r\n            clientX: clientX, clientY: clientY,\r\n            screenX: window.screenX + clientX, screenY: window.screenY + clientY,\r\n            button: button\r\n        });\r\n        const target = document.elementFromPoint(clientX,clientY) || el || document.body;\r\n        try { target.dispatchEvent(ev); } catch(e) { /* ignore */ }\r\n    }\r\n\r\n    function pathPoints(startX, startY, endX, endY, steps) {\r\n        const pts = [];\r\n        for (let i=1;i<=steps;i++){\r\n            const t = i/steps;\r\n            const ease = t*(2-t);\r\n            const x = Math.round(startX + (endX - startX)*ease + (Math.random()-0.5)*2);\r\n            const y = Math.round(startY + (endY - startY)*ease + (Math.random()-0.5)*2);\r\n            pts.push({x,y});\r\n        }\r\n        return pts;\r\n    }\r\n\r\n    async function humanMoveAndLeftClick(el, options = {}) {\r\n        if (!el) return;\r\n        const rect = el.getBoundingClientRect();\r\n        const targetX = options.x ?? Math.round(rect.left + rect.width*(0.3 + Math.random()*0.4));\r\n        const targetY = options.y ?? Math.round(rect.top + rect.height*(0.3 + Math.random()*0.4));\r\n        const startX = options.startX ?? lastMouse.x;\r\n        const startY = options.startY ?? lastMouse.y;\r\n        const steps = options.steps ?? Math.max(2, Math.floor(4 + Math.random()*6));\r\n        const pts = pathPoints(startX, startY, targetX, targetY, steps);\r\n        for (const p of pts) {\r\n            dispatchMouseEvent(el, 'mousemove', p.x, p.y);\r\n            await sleep(Math.floor(8 + Math.random()*22));\r\n        }\r\n        dispatchMouseEvent(el, 'mousemove', targetX + (Math.random()>0.5?1:-1), targetY + (Math.random()>0.5?1:-1));\r\n        await sleep(Math.floor(18 + Math.random()*40));\r\n        dispatchMouseEvent(el, 'mousedown', targetX, targetY, 0);\r\n        await sleep(Math.floor(40 + Math.random()*100));\r\n        dispatchMouseEvent(el, 'mouseup', targetX, targetY, 0);\r\n        dispatchMouseEvent(el, 'click', targetX, targetY, 0);\r\n    }\r\n\r\n    window.__humanMouse = {\r\n        humanMoveAndLeftClick: humanMoveAndLeftClick\r\n    };\r\n})();\r\n";
            ((IJavaScriptExecutor)driver).ExecuteScript(script, Array.Empty<object>());
            _injected = true;
        }
    }
    private string ExtractIdFromSelector(string sel)
    {
        try
        {
            string[] array = sel.Split(new char[3] { ' ', ',', '>' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i].Trim();
                int num = text.IndexOf('#');
                if (num >= 0)
                {
                    string text2 = text.Substring(num + 1).Trim();
                    int num2 = text2.IndexOfAny(new char[3] { '.', '[', ':' });
                    if (num2 > 0)
                    {
                        text2 = text2.Substring(0, num2);
                    }
                    return text2;
                }
            }
        }
        catch
        {
        }
        return null;
    }

    private IWebElement FindBestInput(UndetectedChromeDriver _driver, string inputSelector)
    {
        try
        {
            List<IWebElement> list = (from e in _driver.FindElements(By.CssSelector(inputSelector)).ToList()
                                      where e.Displayed && e.Enabled
                                      select e).ToList();
            if (list.Count == 1)
            {
                return list[0];
            }
            string value = ExtractIdFromSelector(inputSelector);
            if (!string.IsNullOrEmpty(value))
            {
                foreach (IWebElement item in list)
                {
                    try
                    {
                        string text = ((IJavaScriptExecutor)_driver).ExecuteScript("var el = arguments[0]; var p = el.closest('p-autocomplete'); return p ? p.id : null;", new object[1] { item }) as string;
                        if (!string.IsNullOrEmpty(text) && text.Equals(value, StringComparison.OrdinalIgnoreCase))
                        {
                            return item;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            string text2 = (inputSelector ?? "").ToLower();
            string value2 = null;
            if (text2.Contains("from") || text2.Contains("enter from") || text2.Contains("origin"))
            {
                value2 = "from";
            }
            if (text2.Contains("to") || text2.Contains("enter to") || text2.Contains("destination"))
            {
                value2 = "to";
            }
            if (!string.IsNullOrEmpty(value2))
            {
                foreach (IWebElement item2 in list)
                {
                    try
                    {
                        if ((item2.GetAttribute("aria-label") ?? "").ToLower().Contains(value2))
                        {
                            return item2;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            if (list.Count > 1)
            {
                object[] array = list.ToArray();
                object[] array2 = array;
                object[] args = array2;
                object value3 = ((IJavaScriptExecutor)_driver).ExecuteScript("\r\n                    var els = arguments[0];\r\n                    var vw = window.innerWidth/2, vh = window.innerHeight/2;\r\n                    var best = {i:0,d:1e9};\r\n                    for(var i=0;i<els.length;i++){\r\n                      try{\r\n                        var r = els[i].getBoundingClientRect();\r\n                        var cx = r.left + r.width/2, cy = r.top + r.height/2;\r\n                        var d = Math.hypot(cx - vw, cy - vh);\r\n                        if(d < best.d){ best.d = d; best.i = i; }\r\n                      }catch(e){}\r\n                    }\r\n                    return best.i;\r\n                    ", args);
                try
                {
                    int num = Convert.ToInt32(value3);
                    if (num >= 0 && num < list.Count)
                    {
                        return list[num];
                    }
                }
                catch
                {
                }
            }
            if (list.Count > 0)
            {
                return list[0];
            }
        }
        catch
        {
        }
        try
        {
            IWebElement webElement = _driver.FindElement(By.CssSelector(inputSelector)).FindElements(By.CssSelector("input")).FirstOrDefault((IWebElement e) => e.Displayed && e.Enabled);
            if (webElement != null)
            {
                return webElement;
            }
        }
        catch
        {
        }
        return null;
    }

    internal bool SelectDropdownOptionByMouse(UndetectedChromeDriver _driver, string dropdownSelector, string optionText, int timeoutSeconds = 8)
    {
        InjectIfNeeded(_driver);
        Random random = new Random();
        try
        {
            IWebElement webElement = null;
            try
            {
                webElement = FindBestInput(_driver, dropdownSelector);
            }
            catch
            {
                webElement = null;
            }
            if (webElement != null && webElement.TagName.Equals("input", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    IWebElement webElement2 = webElement.FindElement(By.XPath("ancestor::p-dropdown | ancestor::div[contains(@class,'ui-dropdown')]"));
                    if (webElement2 != null)
                    {
                        webElement = webElement2;
                    }
                }
                catch
                {
                }
            }
            if (webElement == null)
            {
                try
                {
                    webElement = _driver.FindElement(By.CssSelector(dropdownSelector));
                }
                catch
                {
                }
            }
            if (webElement == null)
            {
                Console.WriteLine("SelectDropdownOptionByMouse: dropdown wrapper not found for selector: " + dropdownSelector);
                return false;
            }
            IWebElement webElement3 = null;
            try
            {
                webElement3 = webElement.FindElements(By.CssSelector(".ui-dropdown-trigger, .ui-dropdown-label-container, .ui-dropdown")).FirstOrDefault((IWebElement e) => e.Displayed);
            }
            catch
            {
            }
            if (webElement3 == null)
            {
                try
                {
                    webElement3 = webElement.FindElements(By.CssSelector("div,span,button")).FirstOrDefault((IWebElement e) => e.Displayed && (e.GetAttribute("role") == "button" || (e.GetAttribute("class")?.Contains("trigger") ?? false)));
                }
                catch
                {
                }
            }
            if (webElement3 == null)
            {
                webElement3 = webElement;
            }
            try
            {
                ((IJavaScriptExecutor)_driver).ExecuteAsyncScript("\r\nvar el = arguments[0], cb = arguments[arguments.length-1];\r\nif(window.__humanMouse && window.__humanMouse.humanMoveAndLeftClick){\r\n    window.__humanMouse.humanMoveAndLeftClick(el).then(()=>cb(true)).catch(()=>cb(false));\r\n}else{ try{ el.click(); cb(true);}catch(e){cb(false);} }", new object[1] { webElement3 });
            }
            catch
            {
                try
                {
                    webElement3.Click();
                }
                catch
                {
                }
            }
            Thread.Sleep(random.Next(180, 300));
            WebDriverWait webDriverWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds));
            IWebElement webElement4 = null;
            try
            {
                webElement4 = webDriverWait.Until(delegate (IWebDriver d)
                {
                    string[] array = new string[4] { ".ui-dropdown-panel", ".ui-dropdown-items-wrapper", ".ui-dropdown-list", ".ui-dropdown-panel.ui-widget" };
                    string[] array2 = array;
                    string[] array3 = array2;
                    foreach (string cssSelectorToFind in array3)
                    {
                        try
                        {
                            IWebElement webElement5 = d.FindElements(By.CssSelector(cssSelectorToFind)).FirstOrDefault((IWebElement x) => x.Displayed);
                            if (webElement5 != null)
                            {
                                return webElement5;
                            }
                        }
                        catch
                        {
                        }
                    }
                    try
                    {
                        IWebElement webElement6 = d.FindElements(By.CssSelector("ul[role='listbox'], ul.ui-dropdown-items")).FirstOrDefault((IWebElement x) => x.Displayed);
                        if (webElement6 != null)
                        {
                            return webElement6;
                        }
                    }
                    catch
                    {
                    }
                    return (IWebElement?)null;
                });
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("SelectDropdownOptionByMouse: dropdown panel did not appear.");
                return false;
            }
            if (webElement4 == null)
            {
                Console.WriteLine("SelectDropdownOptionByMouse: panel null after wait.");
                return false;
            }
            IWebElement webElement7 = null;
            try
            {
                string[] array4 = new string[7]
                {
                    ".//li[normalize-space(.) = \"" + optionText + "\"]",
                    ".//li[.//span[normalize-space(.) = \"" + optionText + "\"]]",
                    ".//li[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), '" + optionText.ToLower() + "')]",
                    ".//*[(@role='option' or name() = 'li' or name() = 'p-dropdownitem') and normalize-space(.) = \"" + optionText + "\"]",
                    ".//*[normalize-space(@aria-label) = \"" + optionText + "\"]",
                    ".//span[normalize-space(.) = \"" + optionText + "\"]",
                    ".//a[normalize-space(.) = \"" + optionText + "\"]"
                };
                string[] array5 = array4;
                string[] array6 = array5;
                foreach (string xpathToFind in array6)
                {
                    try
                    {
                        IWebElement webElement8 = webElement4.FindElements(By.XPath(xpathToFind)).FirstOrDefault((IWebElement e) => e.Displayed);
                        if (webElement8 != null)
                        {
                            webElement7 = webElement8;
                            break;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            if (webElement7 == null)
            {
                try
                {
                    foreach (IWebElement item in webElement4.FindElements(By.CssSelector("li, .ui-dropdown-item, p-dropdownitem, [role='option']")))
                    {
                        try
                        {
                            string text = (item.Text ?? "").Trim();
                            string a = (item.GetAttribute("aria-label") ?? "").Trim();
                            if (string.Equals(text, optionText, StringComparison.OrdinalIgnoreCase) || string.Equals(a, optionText, StringComparison.OrdinalIgnoreCase) || text.IndexOf(optionText, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                webElement7 = item;
                                break;
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                }
            }
            if (webElement7 == null)
            {
                Console.WriteLine("SelectDropdownOptionByMouse: option '" + optionText + "' not found in panel.");
                return false;
            }
            try
            {
                ((IJavaScriptExecutor)_driver).ExecuteAsyncScript("\r\nvar el = arguments[0], cb = arguments[arguments.length-1];\r\nif(window.__humanMouse && window.__humanMouse.humanMoveAndLeftClick){\r\n    window.__humanMouse.humanMoveAndLeftClick(el).then(()=>cb(true)).catch(()=>cb(false));\r\n}else{ try{ el.click(); cb(true);}catch(e){cb(false);} }", new object[1] { webElement7 });
            }
            catch
            {
                try
                {
                    webElement7.Click();
                }
                catch
                {
                }
            }
            Thread.Sleep(random.Next(120, 260));
            try
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("if(document.activeElement) document.activeElement.blur(); document.body.click();", Array.Empty<object>());
            }
            catch
            {
            }
            return true;
        }
        catch (Exception ex2)
        {
            Console.WriteLine("SelectDropdownOptionByMouse error: " + ex2);
            return false;
        }
    }

    internal bool AutocompleteTypeAndSelectFirst(UndetectedChromeDriver _driver, By inputBy, string text, int timeoutSeconds = 12)
    {
        InjectIfNeeded(_driver);
        WebDriverWait webDriverWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds));
        Random random = new Random();
        try
        {
            IWebElement webElement = webDriverWait.Until(ExpectedConditions.ElementIsVisible(inputBy));
            if (webElement == null)
            {
                return false;
            }
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", new object[1] { webElement });
            Thread.Sleep(random.Next(150, 350));
            string script = "\r\n                (function(el){\r\n                  function fire(type,x,y){\r\n                    var ev = new MouseEvent(type, { bubbles:true, cancelable:true, view:window, clientX: x, clientY: y, button:0});\r\n                    (document.elementFromPoint(x,y) || el).dispatchEvent(ev);\r\n                  }\r\n                  var rect = el.getBoundingClientRect();\r\n                  var endX = Math.round(rect.left + rect.width/2);\r\n                  var endY = Math.round(rect.top + rect.height/2);\r\n                  var startX = Math.floor(Math.random()*window.innerWidth);\r\n                  var startY = Math.floor(Math.random()*window.innerHeight);\r\n                  var cx = Math.round((startX + endX)/2 + (Math.random()*60 - 30));\r\n                  var cy = Math.round((startY + endY)/2 + (Math.random()*60 - 30));\r\n                  var steps = 12;\r\n                  for(var i=0;i<=steps;i++){\r\n                    var t = i/steps;\r\n                    var x = Math.round(Math.pow(1-t,2)*startX + 2*(1-t)*t*cx + Math.pow(t,2)*endX);\r\n                    var y = Math.round(Math.pow(1-t,2)*startY + 2*(1-t)*t*cy + Math.pow(t,2)*endY);\r\n                    fire('mousemove', x, y);\r\n                  }\r\n                  fire('mousedown', endX, endY);\r\n                  fire('mouseup', endX, endY);\r\n                  fire('click', endX, endY);\r\n                  return true;\r\n                })(arguments[0]);\r\n                ";
            try
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript(script, new object[1] { webElement });
            }
            catch
            {
                try
                {
                    webElement.Click();
                }
                catch
                {
                }
            }
            Thread.Sleep(random.Next(120, 260));
            try
            {
                webElement.SendKeys(OpenQA.Selenium.Keys.Control + "a");
                Thread.Sleep(60);
                webElement.SendKeys(OpenQA.Selenium.Keys.Backspace);
            }
            catch
            {
            }
            Thread.Sleep(random.Next(80, 150));
            for (int i = 0; i < text.Length; i++)
            {
                webElement.SendKeys(text[i].ToString());
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].dispatchEvent(new Event('input',{bubbles:true}));", new object[1] { webElement });
                Thread.Sleep(random.Next(60, 140));
            }
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].focus(); arguments[0].dispatchEvent(new Event('input',{bubbles:true}));", new object[1] { webElement });
            Thread.Sleep(random.Next(160, 340));
            int num = 3;
            int num2 = 0;
            while (true)
            {
                if (num2 < num)
                {
                    IWebElement webElement2 = null;
                    DateTime dateTime = DateTime.UtcNow.AddSeconds(timeoutSeconds);
                    while (DateTime.UtcNow < dateTime)
                    {
                        try
                        {
                            string attribute = webElement.GetAttribute("aria-controls");
                            IWebElement webElement3 = null;
                            if (!string.IsNullOrEmpty(attribute))
                            {
                                IWebElement webElement4 = _driver.FindElements(By.Id(attribute)).FirstOrDefault((IWebElement e) => e.Displayed);
                                if (webElement4 != null)
                                {
                                    webElement3 = webElement4;
                                }
                            }
                            if (webElement3 == null)
                            {
                                webElement3 = _driver.FindElements(By.CssSelector(".ui-autocomplete-items, .ui-autocomplete-panel, .p-autocomplete-panel, ul[role='listbox']")).FirstOrDefault((IWebElement e) => e.Displayed);
                            }
                            if (webElement3 != null)
                            {
                                List<IWebElement> list = (from e in webElement3.FindElements(By.CssSelector("li[role='option'], li, [role='option'], .ui-autocomplete-item, .p-autocomplete-item"))
                                                          where e.Displayed && SafeEnabled(e)
                                                          select e).ToList();
                                if (list.Count > 0)
                                {
                                    webElement2 = list[0];
                                    break;
                                }
                            }
                        }
                        catch (StaleElementReferenceException)
                        {
                        }
                        catch
                        {
                        }
                        Thread.Sleep(120);
                    }
                    if (webElement2 == null)
                    {
                        Thread.Sleep(180);
                        try
                        {
                            webElement.SendKeys(OpenQA.Selenium.Keys.ArrowDown);
                            Thread.Sleep(120);
                            webElement.SendKeys(OpenQA.Selenium.Keys.Enter);
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            new Actions(_driver).MoveToElement(webElement2).Pause(TimeSpan.FromMilliseconds(random.Next(60, 160))).Click()
                                .Perform();
                        }
                        catch
                        {
                            try
                            {
                                ((IJavaScriptExecutor)_driver).ExecuteScript(script, new object[1] { webElement2 });
                            }
                            catch
                            {
                                try
                                {
                                    webElement.SendKeys(OpenQA.Selenium.Keys.ArrowDown);
                                    Thread.Sleep(80);
                                    webElement.SendKeys(OpenQA.Selenium.Keys.Enter);
                                    goto end_IL_0373;
                                }
                                catch
                                {
                                    goto end_IL_0373;
                                }
                            end_IL_0373:;
                            }
                        }
                    }
                    Thread.Sleep(220);
                    string text2 = "";
                    try
                    {
                        text2 = webElement.GetAttribute("value") ?? webElement.Text ?? "";
                    }
                    catch
                    {
                    }
                    string text3 = "";
                    try
                    {
                        text3 = webElement.GetAttribute("aria-activedescendant") ?? "";
                    }
                    catch
                    {
                    }
                    if (!string.IsNullOrEmpty(text2) && !text2.Equals(text, StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                    if (!string.IsNullOrEmpty(text3))
                    {
                        try
                        {
                            if (_driver.FindElement(By.Id(text3)) != null)
                            {
                                return true;
                            }
                        }
                        catch
                        {
                        }
                    }
                    try
                    {
                        webElement.SendKeys(OpenQA.Selenium.Keys.ArrowDown);
                        Thread.Sleep(120);
                        webElement.SendKeys(OpenQA.Selenium.Keys.Enter);
                        Thread.Sleep(200);
                        text2 = webElement.GetAttribute("value") ?? "";
                        if (!string.IsNullOrEmpty(text2) && !text2.Equals(text, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                    catch
                    {
                    }
                    Thread.Sleep(300 + random.Next(0, 200));
                    num2++;
                    continue;
                }
                Console.WriteLine("Autocomplete selection attempts exhausted.");
                return false;
            }
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("Timeout waiting for autocomplete.");
            return false;
        }
        catch (Exception ex3)
        {
            Console.WriteLine("Error: " + ex3.Message);
            return false;
        }
    }
    private bool SafeEnabled(IWebElement el)
    {
        try
        {
            return el?.Enabled ?? false;
        }
        catch
        {
            return false;
        }
    }
    internal bool ScrollElementToCenter_Play(IPage page, string selector, int timeout = 10000)
    {
        try
        {
            ILocator first = page.Locator(selector, (PageLocatorOptions)null).First;
            first.WaitForAsync(new LocatorWaitForOptions
            {
                State = (WaitForSelectorState)2,
                Timeout = timeout
            }).GetAwaiter().GetResult();
            first.EvaluateAsync("\r\n            el => {\r\n                el.scrollIntoView({\r\n                    behavior: 'smooth',\r\n                    block: 'center',\r\n                    inline: 'center'\r\n                });\r\n            }\r\n        ", (object)null, (LocatorEvaluateOptions)null).GetAwaiter().GetResult();
            Thread.Sleep(800);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("ScrollElementToCenter Error: " + ex.Message);
            return false;
        }
    }
    internal bool SelectQuotaRealMouse_Play(IPage page, string dropdownSelector, string optionText, int timeout = 10000)
    {
        try
        {
            ILocator first = page.Locator(dropdownSelector, (PageLocatorOptions)null).First;
            first.WaitForAsync(new LocatorWaitForOptions
            {
                State = (WaitForSelectorState)2,
                Timeout = timeout
            }).GetAwaiter().GetResult();
            first.EvaluateAsync("\r\n            el => {\r\n                el.scrollIntoView({\r\n                    behavior: 'instant',\r\n                    block: 'center',\r\n                    inline: 'center'\r\n                });\r\n            }\r\n        ", (object)null, (LocatorEvaluateOptions)null).GetAwaiter().GetResult();
            Thread.Sleep(500);
            LocatorBoundingBoxResult result = first.BoundingBoxAsync((LocatorBoundingBoxOptions)null).GetAwaiter().GetResult();
            if (result == null)
            {
                Console.WriteLine("Quota field BoundingBox not found.");
                return false;
            }
            float num = result.X + result.Width / 2f;
            float num2 = result.Y + result.Height / 2f;
            Console.WriteLine($"Quota Field => X:{num} Y:{num2}");
            page.Mouse.MoveAsync(num, num2, new MouseMoveOptions
            {
                Steps = 25
            }).GetAwaiter().GetResult();
            Thread.Sleep(rnd.Next(180, 350));
            page.Mouse.DownAsync((MouseDownOptions)null).GetAwaiter().GetResult();
            Thread.Sleep(rnd.Next(60, 130));
            page.Mouse.UpAsync((MouseUpOptions)null).GetAwaiter().GetResult();
            Thread.Sleep(rnd.Next(500, 800));
            ILocator last = page.Locator(".ui-dropdown-panel:visible, .p-dropdown-panel:visible, .p-select-overlay:visible", (PageLocatorOptions)null).Last;
            last.WaitForAsync(new LocatorWaitForOptions
            {
                State = (WaitForSelectorState)2,
                Timeout = timeout
            }).GetAwaiter().GetResult();
            Thread.Sleep(300);
            ILocator val = last.Locator("li[role='option'], [role='option'], .ui-dropdown-item, .p-dropdown-item, .p-select-option", (LocatorLocatorOptions)null);
            int result2 = val.CountAsync().GetAwaiter().GetResult();
            ILocator val2 = null;
            for (int i = 0; i < result2; i++)
            {
                ILocator val3 = val.Nth(i);
                try
                {
                    if (val3.IsVisibleAsync((LocatorIsVisibleOptions)null).GetAwaiter().GetResult())
                    {
                        string text = val3.InnerTextAsync((LocatorInnerTextOptions)null).GetAwaiter().GetResult()
                            .Trim();
                        Console.WriteLine("Quota Option: [" + text + "]");
                        if (text.Equals(optionText.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            val2 = val3;
                            break;
                        }
                    }
                }
                catch
                {
                }
            }
            if (val2 == null)
            {
                Console.WriteLine("Quota option not found: " + optionText);
                return false;
            }
            val2.ScrollIntoViewIfNeededAsync((LocatorScrollIntoViewIfNeededOptions)null).GetAwaiter().GetResult();
            Thread.Sleep(rnd.Next(150, 300));
            LocatorBoundingBoxResult result3 = val2.BoundingBoxAsync((LocatorBoundingBoxOptions)null).GetAwaiter().GetResult();
            if (result3 == null)
            {
                Console.WriteLine("Quota option BoundingBox not found.");
                return false;
            }
            float num3 = result3.X + result3.Width / 2f;
            float num4 = result3.Y + result3.Height / 2f;
            Console.WriteLine("Quota Option Click => " + $"{optionText} X:{num3} Y:{num4}");
            page.Mouse.MoveAsync(num3, num4, new MouseMoveOptions
            {
                Steps = 25
            }).GetAwaiter().GetResult();
            Thread.Sleep(rnd.Next(180, 350));
            page.Mouse.DownAsync((MouseDownOptions)null).GetAwaiter().GetResult();
            Thread.Sleep(rnd.Next(60, 130));
            page.Mouse.UpAsync((MouseUpOptions)null).GetAwaiter().GetResult();
            Thread.Sleep(700);
            string text2 = first.InnerTextAsync((LocatorInnerTextOptions)null).GetAwaiter().GetResult()
                .Trim();
            Console.WriteLine("Selected Quota: " + text2);
            if (text2.IndexOf(optionText.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            Console.WriteLine("Quota select nahi hua, retry kar raha hoon...");
            Thread.Sleep(500);
            result = first.BoundingBoxAsync((LocatorBoundingBoxOptions)null).GetAwaiter().GetResult();
            if (result == null)
            {
                return false;
            }
            num = result.X + result.Width / 2f + (float)rnd.Next(-3, 4);
            num2 = result.Y + result.Height / 2f + (float)rnd.Next(-2, 3);
            page.Mouse.MoveAsync(num, num2, new MouseMoveOptions
            {
                Steps = 20
            }).GetAwaiter().GetResult();
            Thread.Sleep(200);
            page.Mouse.ClickAsync(num, num2, (MouseClickOptions)null).GetAwaiter().GetResult();
            Thread.Sleep(500);
            last = page.Locator(".ui-dropdown-panel:visible, .p-dropdown-panel:visible, .p-select-overlay:visible", (PageLocatorOptions)null).Last;
            last.WaitForAsync(new LocatorWaitForOptions
            {
                State = (WaitForSelectorState)2,
                Timeout = 3000f
            }).GetAwaiter().GetResult();
            val2 = last.Locator("li[role='option'], [role='option'], .ui-dropdown-item, .p-dropdown-item, .p-select-option", (LocatorLocatorOptions)null).Filter(new LocatorFilterOptions
            {
                HasTextString = optionText
            }).First;
            val2.WaitForAsync(new LocatorWaitForOptions
            {
                State = (WaitForSelectorState)2,
                Timeout = 3000f
            }).GetAwaiter().GetResult();
            val2.ScrollIntoViewIfNeededAsync((LocatorScrollIntoViewIfNeededOptions)null).GetAwaiter().GetResult();
            Thread.Sleep(200);
            result3 = val2.BoundingBoxAsync((LocatorBoundingBoxOptions)null).GetAwaiter().GetResult();
            if (result3 == null)
            {
                return false;
            }
            num3 = result3.X + result3.Width / 2f + (float)rnd.Next(-3, 4);
            num4 = result3.Y + result3.Height / 2f + (float)rnd.Next(-2, 3);
            page.Mouse.MoveAsync(num3, num4, new MouseMoveOptions
            {
                Steps = 22
            }).GetAwaiter().GetResult();
            Thread.Sleep(rnd.Next(150, 300));
            page.Mouse.DownAsync((MouseDownOptions)null).GetAwaiter().GetResult();
            Thread.Sleep(rnd.Next(60, 120));
            page.Mouse.UpAsync((MouseUpOptions)null).GetAwaiter().GetResult();
            Thread.Sleep(700);
            text2 = first.InnerTextAsync((LocatorInnerTextOptions)null).GetAwaiter().GetResult()
                .Trim();
            Console.WriteLine("Final Quota: " + text2);
            return text2.IndexOf(optionText.Trim(), StringComparison.OrdinalIgnoreCase) >= 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("SelectQuotaRealMouse_Play Error: " + ex.ToString());
            return false;
        }
    }

    internal bool SetDateHuman_Play(IPage page, string selector, string value, int timeout = 15000)
    {
        try
        {
            if (!DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                Console.WriteLine("Invalid date: " + value);
                return false;
            }
            Console.WriteLine("Target Date: " + result.ToString("dd/MM/yyyy"));
            ILocator first = page.Locator(selector, (PageLocatorOptions)null).First;
            first.WaitForAsync(new LocatorWaitForOptions
            {
                State = (WaitForSelectorState)2,
                Timeout = timeout
            }).GetAwaiter().GetResult();
            first.EvaluateAsync("\r\n            el => el.scrollIntoView({\r\n                behavior: 'instant',\r\n                block: 'center',\r\n                inline: 'center'\r\n            });\r\n        ", (object)null, (LocatorEvaluateOptions)null).GetAwaiter().GetResult();
            Thread.Sleep(400);
            LocatorBoundingBoxResult result2 = first.BoundingBoxAsync((LocatorBoundingBoxOptions)null).GetAwaiter().GetResult();
            if (result2 == null)
            {
                Console.WriteLine("Date input box not found.");
                return false;
            }
            float num = result2.X + result2.Width / 2f;
            float num2 = result2.Y + result2.Height / 2f;
            page.Mouse.MoveAsync(num, num2, new MouseMoveOptions
            {
                Steps = 25
            }).GetAwaiter().GetResult();
            Thread.Sleep(rnd.Next(150, 350));
            page.Mouse.DownAsync((MouseDownOptions)null).GetAwaiter().GetResult();
            Thread.Sleep(rnd.Next(60, 120));
            page.Mouse.UpAsync((MouseUpOptions)null).GetAwaiter().GetResult();
            Thread.Sleep(700);
            ILocator last = page.Locator(".ui-datepicker:visible, .p-datepicker:visible", (PageLocatorOptions)null).Last;
            last.WaitForAsync(new LocatorWaitForOptions
            {
                State = (WaitForSelectorState)2,
                Timeout = 7000f
            }).GetAwaiter().GetResult();
            bool flag = false;
            for (int i = 1; i <= 36; i++)
            {
                last = page.Locator(".ui-datepicker:visible, .p-datepicker:visible", (PageLocatorOptions)null).Last;
                last.WaitForAsync(new LocatorWaitForOptions
                {
                    State = (WaitForSelectorState)2,
                    Timeout = 5000f
                }).GetAwaiter().GetResult();
                string text = "";
                try
                {
                    ILocator first2 = last.Locator(".ui-datepicker-title, .p-datepicker-title", (LocatorLocatorOptions)null).First;
                    if (first2.CountAsync().GetAwaiter().GetResult() > 0)
                    {
                        text = first2.InnerTextAsync((LocatorInnerTextOptions)null).GetAwaiter().GetResult()
                            .Trim();
                    }
                }
                catch
                {
                }
                if (string.IsNullOrWhiteSpace(text))
                {
                    try
                    {
                        text = last.EvaluateAsync<string>("\r\n                            el => {\r\n                                let title =\r\n                                    el.querySelector(\r\n                                        '.ui-datepicker-title, .p-datepicker-title'\r\n                                    );\r\n\r\n                                return title\r\n                                    ? title.innerText.trim()\r\n                                    : '';\r\n                            }\r\n                        ", (object)null, (LocatorEvaluateOptions)null).GetAwaiter().GetResult();
                    }
                    catch
                    {
                    }
                }
                Console.WriteLine($"Month Attempt {i}: " + $"Current=[{text}] Target=[{result:MMMM yyyy}]");
                DateTime result3;
                bool flag2 = DateTime.TryParseExact(text, new string[2] { "MMMM yyyy", "MMM yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out result3);
                if (!flag2)
                {
                    try
                    {
                        string result4 = last.InnerTextAsync((LocatorInnerTextOptions)null).GetAwaiter().GetResult();
                        string[] array = new string[12]
                        {
                        "January", "February", "March", "April", "May", "June", "July", "August", "September", "October",
                        "November", "December"
                        };
                        string[] array2 = array;
                        foreach (string value2 in array2)
                        {
                            if (result4.IndexOf(value2, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                string value3 = result.Year.ToString();
                                if (result4.Contains(value3))
                                {
                                    result3 = new DateTime(result.Year, Array.IndexOf(array, value2) + 1, 1);
                                    flag2 = true;
                                    break;
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                if (flag2 && result3.Year == result.Year && result3.Month == result.Month)
                {
                    flag = true;
                    Console.WriteLine("Target month found: " + result.ToString("MMMM yyyy"));
                    break;
                }
                bool flag3 = !flag2 || result.Year > result3.Year || (result.Year == result3.Year && result.Month > result3.Month);
                string text2 = (flag3 ? ".ui-datepicker-next:visible, .p-datepicker-next:visible" : ".ui-datepicker-prev:visible, .p-datepicker-prev:visible");
                ILocator last2 = page.Locator(text2, (PageLocatorOptions)null).Last;
                last2.WaitForAsync(new LocatorWaitForOptions
                {
                    State = (WaitForSelectorState)2,
                    Timeout = 5000f
                }).GetAwaiter().GetResult();
                last2.EvaluateAsync("\r\n                el => el.scrollIntoView({\r\n                    behavior: 'instant',\r\n                    block: 'center',\r\n                    inline: 'center'\r\n                });\r\n            ", (object)null, (LocatorEvaluateOptions)null).GetAwaiter().GetResult();
                Thread.Sleep(250);
                LocatorBoundingBoxResult result5 = last2.BoundingBoxAsync((LocatorBoundingBoxOptions)null).GetAwaiter().GetResult();
                if (result5 == null)
                {
                    Console.WriteLine("Navigation button bounding box missing.");
                    continue;
                }
                float num3 = result5.X + result5.Width / 2f;
                float num4 = result5.Y + result5.Height / 2f;
                Console.WriteLine("Mouse " + (flag3 ? "NEXT" : "PREVIOUS") + " => " + $"X:{num3} Y:{num4}");
                page.Mouse.MoveAsync(num3, num4, new MouseMoveOptions
                {
                    Steps = rnd.Next(18, 30)
                }).GetAwaiter().GetResult();
                Thread.Sleep(rnd.Next(120, 300));
                page.Mouse.DownAsync((MouseDownOptions)null).GetAwaiter().GetResult();
                Thread.Sleep(rnd.Next(60, 130));
                page.Mouse.UpAsync((MouseUpOptions)null).GetAwaiter().GetResult();
                Thread.Sleep(rnd.Next(500, 900));
            }
            if (!flag)
            {
                Console.WriteLine("Target month not reached: " + result.ToString("MMMM yyyy"));
                return false;
            }
            last = page.Locator(".ui-datepicker:visible, .p-datepicker:visible", (PageLocatorOptions)null).Last;
            last.WaitForAsync(new LocatorWaitForOptions
            {
                State = (WaitForSelectorState)2,
                Timeout = 5000f
            }).GetAwaiter().GetResult();
            ILocator val = last.Locator("td:not(.ui-datepicker-other-month):not(.p-datepicker-other-month)", (LocatorLocatorOptions)null);
            int result6 = val.CountAsync().GetAwaiter().GetResult();
            ILocator val2 = null;
            string text3 = result.Day.ToString();
            for (int k = 0; k < result6; k++)
            {
                ILocator val3 = val.Nth(k);
                string text4 = "";
                try
                {
                    text4 = val3.InnerTextAsync((LocatorInnerTextOptions)null).GetAwaiter().GetResult()
                        .Trim();
                }
                catch
                {
                }
                if (text4 == text3)
                {
                    val2 = val3;
                    break;
                }
            }
            if (val2 == null)
            {
                Console.WriteLine("Exact date cell not found: " + text3);
                return false;
            }
            if (!val2.IsVisibleAsync((LocatorIsVisibleOptions)null).GetAwaiter().GetResult())
            {
                Console.WriteLine("Date cell visible nahi hai.");
                return false;
            }
            ILocator val4 = val2;
            try
            {
                ILocator first3 = val2.Locator("a", (LocatorLocatorOptions)null).First;
                if (first3.CountAsync().GetAwaiter().GetResult() > 0)
                {
                    val4 = first3;
                }
            }
            catch
            {
            }
            val4.EvaluateAsync("\r\n            el => el.scrollIntoView({\r\n                behavior: 'instant',\r\n                block: 'center',\r\n                inline: 'center'\r\n            });\r\n        ", (object)null, (LocatorEvaluateOptions)null).GetAwaiter().GetResult();
            Thread.Sleep(300);
            LocatorBoundingBoxResult result7 = val4.BoundingBoxAsync((LocatorBoundingBoxOptions)null).GetAwaiter().GetResult();
            if (result7 == null)
            {
                Console.WriteLine("Date bounding box not found.");
                return false;
            }
            float num5 = result7.X + result7.Width / 2f;
            float num6 = result7.Y + result7.Height / 2f;
            Console.WriteLine($"DATE CLICK => {result:dd/MM/yyyy} " + $"X:{num5} Y:{num6}");
            page.Mouse.MoveAsync(num5, num6, new MouseMoveOptions
            {
                Steps = rnd.Next(20, 30)
            }).GetAwaiter().GetResult();
            Thread.Sleep(rnd.Next(150, 350));
            page.Mouse.DownAsync((MouseDownOptions)null).GetAwaiter().GetResult();
            Thread.Sleep(rnd.Next(70, 130));
            page.Mouse.UpAsync((MouseUpOptions)null).GetAwaiter().GetResult();
            Thread.Sleep(rnd.Next(700, 1000));
            string text5 = "";
            try
            {
                text5 = first.InputValueAsync((LocatorInputValueOptions)null).GetAwaiter().GetResult()
                    .Trim();
            }
            catch
            {
            }
            Console.WriteLine("Date Result => Expected:" + value + " Actual:" + text5);
            if (text5.Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            for (int l = 1; l <= 2; l++)
            {
                last = page.Locator(".ui-datepicker:visible, .p-datepicker:visible", (PageLocatorOptions)null).Last;
                ILocator val5 = last.Locator("td:not(.ui-datepicker-other-month):not(.p-datepicker-other-month)", (LocatorLocatorOptions)null);
                int result8 = val5.CountAsync().GetAwaiter().GetResult();
                ILocator val6 = null;
                for (int m = 0; m < result8; m++)
                {
                    ILocator val7 = val5.Nth(m);
                    string text6 = val7.InnerTextAsync((LocatorInnerTextOptions)null).GetAwaiter().GetResult()
                        .Trim();
                    if (text6 == text3)
                    {
                        val6 = val7;
                        break;
                    }
                }
                if (val6 == null)
                {
                    continue;
                }
                ILocator val8 = val6;
                try
                {
                    ILocator first4 = val6.Locator("a", (LocatorLocatorOptions)null).First;
                    if (first4.CountAsync().GetAwaiter().GetResult() > 0)
                    {
                        val8 = first4;
                    }
                }
                catch
                {
                }
                LocatorBoundingBoxResult result9 = val8.BoundingBoxAsync((LocatorBoundingBoxOptions)null).GetAwaiter().GetResult();
                if (result9 != null)
                {
                    float num7 = result9.X + result9.Width / 2f;
                    float num8 = result9.Y + result9.Height / 2f;
                    Console.WriteLine($"Date Retry {l} => " + $"X:{num7} Y:{num8}");
                    page.Mouse.MoveAsync(num7, num8, new MouseMoveOptions
                    {
                        Steps = rnd.Next(18, 28)
                    }).GetAwaiter().GetResult();
                    Thread.Sleep(rnd.Next(120, 250));
                    page.Mouse.DownAsync((MouseDownOptions)null).GetAwaiter().GetResult();
                    Thread.Sleep(rnd.Next(60, 120));
                    page.Mouse.UpAsync((MouseUpOptions)null).GetAwaiter().GetResult();
                    Thread.Sleep(700);
                    text5 = first.InputValueAsync((LocatorInputValueOptions)null).GetAwaiter().GetResult()
                        .Trim();
                    if (text5.Equals(value, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Date successfully selected.");
                        return true;
                    }
                }
            }
            Console.WriteLine("Date selection failed.");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine("SetDateHuman_Play Error: " + ex.ToString());
            return false;
        }
    }
    internal bool HumanLikeClickAndTypeselect_new(IPage page, string selector, string text)
    {
        for (int i = 1; i <= 3; i++)
        {
            try
            {
                ILocator first = page.Locator(selector, (PageLocatorOptions)null).First;
                first.WaitForAsync(new LocatorWaitForOptions
                {
                    State = (WaitForSelectorState)2,
                    Timeout = 10000f
                }).GetAwaiter().GetResult();
                first.ScrollIntoViewIfNeededAsync((LocatorScrollIntoViewIfNeededOptions)null).GetAwaiter().GetResult();
                Thread.Sleep(300);
                LocatorBoundingBoxResult result = first.BoundingBoxAsync((LocatorBoundingBoxOptions)null).GetAwaiter().GetResult();
                if (result == null || result.Width <= 0f || result.Height <= 0f)
                {
                    throw new Exception("Input BoundingBox not available.");
                }
                int num = Math.Max(5, (int)Math.Min(15.0, (double)result.Width * 0.2));
                int num2 = Math.Max(3, (int)Math.Min(8.0, (double)result.Height * 0.2));
                int num3 = (int)result.X + num;
                int num4 = (int)(result.X + result.Width) - num;
                int num5 = (int)result.Y + num2;
                int num6 = (int)(result.Y + result.Height) - num2;
                if (num4 <= num3)
                {
                    num3 = (num4 = (int)(result.X + result.Width / 2f));
                }
                if (num6 <= num5)
                {
                    num5 = (num6 = (int)(result.Y + result.Height / 2f));
                }
                int num7 = rnd.Next(num3, num4 + 1);
                int num8 = rnd.Next(num5, num6 + 1);
                POINT playwrightScreenCoordinates = GetPlaywrightScreenCoordinates(page, num7, num8);
                int x = playwrightScreenCoordinates.X;
                int y = playwrightScreenCoordinates.Y;
                Console.WriteLine($"Attempt {i}: " + $"Viewport=({num7},{num8}) " + $"Screen=({x},{y})");
                GetCursorPos(out var lpPoint);
                HumanMoveToPosition(lpPoint.X, lpPoint.Y, x, y);
                Thread.Sleep(rnd.Next(150, 300));
                GetCursorPos(out var lpPoint2);
                int num9 = Math.Abs(lpPoint2.X - x);
                int num10 = Math.Abs(lpPoint2.Y - y);
                if (num9 > 8 || num10 > 8)
                {
                    Console.WriteLine("Cursor target miss. " + $"Target=({x},{y}) " + $"Actual=({lpPoint2.X},{lpPoint2.Y})");
                    Thread.Sleep(rnd.Next(400, 700));
                    continue;
                }
                mouse_event(2, lpPoint2.X, lpPoint2.Y, 0, 0);
                Thread.Sleep(rnd.Next(40, 90));
                mouse_event(4, lpPoint2.X, lpPoint2.Y, 0, 0);
                Thread.Sleep(300);
                first.PressAsync("Control+A", (LocatorPressOptions)null).GetAwaiter().GetResult();
                Thread.Sleep(100);
                first.PressAsync("Backspace", (LocatorPressOptions)null).GetAwaiter().GetResult();
                Thread.Sleep(200);
                foreach (char c in text)
                {
                    page.Keyboard.TypeAsync(c.ToString(), (KeyboardTypeOptions)null).GetAwaiter().GetResult();
                    Thread.Sleep(rnd.Next(70, 140));
                }
                Thread.Sleep(1000);
                ILocator first2 = page.Locator(".p-autocomplete-item:visible, .ui-autocomplete-item:visible, li[role='option']:visible", (PageLocatorOptions)null).First;
                bool flag = false;
                try
                {
                    first2.WaitForAsync(new LocatorWaitForOptions
                    {
                        State = (WaitForSelectorState)2,
                        Timeout = 3000f
                    }).GetAwaiter().GetResult();
                    flag = true;
                }
                catch
                {
                    flag = false;
                }
                if (flag)
                {
                    try
                    {
                        LocatorBoundingBoxResult result2 = first2.BoundingBoxAsync((LocatorBoundingBoxOptions)null).GetAwaiter().GetResult();
                        if (result2 != null && result2.Width > 0f && result2.Height > 0f)
                        {
                            int viewportX = (int)(result2.X + result2.Width / 2f);
                            int viewportY = (int)(result2.Y + result2.Height / 2f);
                            POINT playwrightScreenCoordinates2 = GetPlaywrightScreenCoordinates(page, viewportX, viewportY);
                            GetCursorPos(out var lpPoint3);
                            HumanMoveToPosition(lpPoint3.X, lpPoint3.Y, playwrightScreenCoordinates2.X, playwrightScreenCoordinates2.Y);
                            Thread.Sleep(rnd.Next(150, 300));
                            GetCursorPos(out var lpPoint4);
                            mouse_event(2, lpPoint4.X, lpPoint4.Y, 0, 0);
                            Thread.Sleep(rnd.Next(40, 90));
                            mouse_event(4, lpPoint4.X, lpPoint4.Y, 0, 0);
                        }
                        else
                        {
                            first2.ClickAsync((LocatorClickOptions)null).GetAwaiter().GetResult();
                        }
                    }
                    catch
                    {
                        first2.EvaluateAsync("el => el.click()", (object)null, (LocatorEvaluateOptions)null).GetAwaiter().GetResult();
                    }
                    Thread.Sleep(500);
                }
                else
                {
                    first.PressAsync("Enter", (LocatorPressOptions)null).GetAwaiter().GetResult();
                    Thread.Sleep(500);
                }
                string value = first.InputValueAsync((LocatorInputValueOptions)null).GetAwaiter().GetResult()
                    .Trim();
                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Autocomplete attempt {i}/3: " + ex.Message);
                Thread.Sleep(rnd.Next(700, 1000));
            }
        }
        return false;
    }

    private POINT GetPlaywrightScreenCoordinates(IPage page, int viewportX, int viewportY)
    {
        double[] result = page.EvaluateAsync<double[]>("\r\n            () => [\r\n                window.screenX,\r\n                window.screenY,\r\n                window.outerWidth,\r\n                window.outerHeight,\r\n                window.innerWidth,\r\n                window.innerHeight\r\n            ]\r\n        ", (object)null).GetAwaiter().GetResult();
        double num = result[0];
        double num2 = result[1];
        double num3 = result[2];
        double num4 = result[3];
        double num5 = result[4];
        double num6 = result[5];
        double num7 = (num3 - num5) / 2.0;
        double num8 = num4 - num6;
        int x = (int)Math.Round(num + num7 + (double)viewportX);
        int y = (int)Math.Round(num2 + num8 + (double)viewportY);
        POINT result2 = default(POINT);
        result2.X = x;
        result2.Y = y;
        return result2;
    }

    internal void HumanLikeClickAndTypeselect_Play(IPage page, By locator, string text)
	{
		try
		{
			string text2 = locator.ToString();
			string oldValue = "By.CssSelector: ";
			string selector = text2.Replace(oldValue, "");
			IElementHandle result = page.QuerySelectorAsync(selector).GetAwaiter().GetResult();
			POINT accurateScreenCoordinates_Play = GetAccurateScreenCoordinates_Play(page, result);
			int x = accurateScreenCoordinates_Play.X;
			int num = accurateScreenCoordinates_Play.Y - 10;
			GetCursorPos(out var lpPoint);
			HumanMoveToPosition(lpPoint.X, lpPoint.Y, x, num);
			GetCursorPos(out var _);
			Thread.Sleep(rnd.Next(100, 250));
			if (IsElementUnderCursor_Play(page, result, x, num))
			{
				string expression = "(el) => { el.focus(); el.value = ''; }";
				page.EvaluateAsync(expression, result).GetAwaiter().GetResult();
				Thread.Sleep(100);
				mouse_event(2, x, num, 0, 0);
				Thread.Sleep(rnd.Next(30, 100));
				mouse_event(4, x, num, 0, 0);
				Thread.Sleep(rnd.Next(150, 300));
				TypeTextWindowsAPI_new(text);
			}
			else
			{
				string expression2 = "el => el.click()";
				page.EvaluateAsync(expression2, result).GetAwaiter().GetResult();
				string expression3 = "(el) => { el.focus(); el.value = ''; }";
				page.EvaluateAsync(expression3, result).GetAwaiter().GetResult();
				Thread.Sleep(100);
				Clipboard.SetText(text);
				Thread.Sleep(150);
				string keys = "^v";
				SendKeys.SendWait(keys);
			}
		}
		catch
		{
			try
			{
				string text3 = locator.ToString();
				string oldValue2 = "By.CssSelector: ";
				string selector2 = text3.Replace(oldValue2, "");
				IElementHandle result2 = page.QuerySelectorAsync(selector2).GetAwaiter().GetResult();
				string expression4 = "el => el.click()";
				page.EvaluateAsync(expression4, result2).GetAwaiter().GetResult();
				string expression5 = "(el) => { el.focus(); el.value = ''; }";
				page.EvaluateAsync(expression5, result2).GetAwaiter().GetResult();
				Thread.Sleep(100);
				Clipboard.SetText(text);
				Thread.Sleep(150);
				string keys2 = "^v";
				SendKeys.SendWait(keys2);
			}
			catch
			{
			}
		}
	}

    internal void HumanLikeClickAndType_Play_Direct(IPage page, By locator,string text)
    {
        try
        {
            string selector = locator.ToString().Replace("By.CssSelector: ", "").Trim();
            IElementHandle element = page.QuerySelectorAsync(selector).GetAwaiter().GetResult();
            if (element == null)
            {
                Console.WriteLine("Element not found: " + selector);
                return;
            }
            element.ScrollIntoViewIfNeededAsync().GetAwaiter().GetResult();
            element.ClickAsync().GetAwaiter().GetResult();
            element.FillAsync(text ?? string.Empty).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                "HumanLikeClickAndType_Play Error: " + ex.Message);
        }
    }



    internal void HumanLikeClickAndType_Play(IPage page, By locator, string text)
    {
        try
        {
            string text2 = locator.ToString();
            string oldValue = "By.CssSelector: ";
            string selector = text2.Replace(oldValue, "");
            IElementHandle result = page.QuerySelectorAsync(selector).GetAwaiter().GetResult();
            POINT accurateScreenCoordinates_Play = GetAccurateScreenCoordinates_Play(page, result);
            int x = accurateScreenCoordinates_Play.X;
            int num = accurateScreenCoordinates_Play.Y - 10;
            GetCursorPos(out var lpPoint);
            HumanMoveToPosition(lpPoint.X, lpPoint.Y, x, num);
            GetCursorPos(out var _);
            Thread.Sleep(rnd.Next(100, 250));
            if (IsElementUnderCursor_Play(page, result, x, num))
            {
                string expression = "(el) => { el.focus(); el.value = ''; }";
                page.EvaluateAsync(expression, result).GetAwaiter().GetResult();
                Thread.Sleep(100);
                mouse_event(2, x, num, 0, 0);
                Thread.Sleep(rnd.Next(30, 100));
                mouse_event(4, x, num, 0, 0);
                Thread.Sleep(rnd.Next(150, 300));
                TypeTextWindowsAPI_new(text);
            }
            else
            {
                string expression2 = "el => el.click()";
                page.EvaluateAsync(expression2, result).GetAwaiter().GetResult();
                string expression3 = "(el) => { el.focus(); el.value = ''; }";
                page.EvaluateAsync(expression3, result).GetAwaiter().GetResult();
                Thread.Sleep(100);
                Clipboard.SetText(text);
                Thread.Sleep(150);
                string keys = "^v";
                SendKeys.SendWait(keys);
            }
        }
        catch
        {
            try
            {
                string text3 = locator.ToString();
                string oldValue2 = "By.CssSelector: ";
                string selector2 = text3.Replace(oldValue2, "");
                IElementHandle result2 = page.QuerySelectorAsync(selector2).GetAwaiter().GetResult();
                string expression4 = "el => el.click()";
                page.EvaluateAsync(expression4, result2).GetAwaiter().GetResult();
                string expression5 = "(el) => { el.focus(); el.value = ''; }";
                page.EvaluateAsync(expression5, result2).GetAwaiter().GetResult();
                Thread.Sleep(100);
                Clipboard.SetText(text);
                Thread.Sleep(150);
                string keys2 = "^v";
                SendKeys.SendWait(keys2);
            }
            catch
            {
            }
        }
    }

    private POINT GetAccurateScreenCoordinates_Play(IPage page, IElementHandle element)
	{
		string expression = "\r\n        (el) => {\r\n\r\n            var rect = el.getBoundingClientRect();\r\n            var offsetRect = el.getBoundingClientRect();\r\n\r\n            var style = window.getComputedStyle(el);\r\n\r\n            var borderLeft = parseFloat(style.borderLeftWidth) || 0;\r\n            var borderTop = parseFloat(style.borderTopWidth) || 0;\r\n\r\n            var paddingLeft = parseFloat(style.paddingLeft) || 0;\r\n            var paddingTop = parseFloat(style.paddingTop) || 0;\r\n\r\n            var clickableLeft = rect.left + borderLeft + paddingLeft;\r\n            var clickableTop = rect.top + borderTop + paddingTop;\r\n\r\n            var clickableWidth = rect.width\r\n                - (borderLeft + parseFloat(style.borderRightWidth) || 0)\r\n                - (paddingLeft + parseFloat(style.paddingRight) || 0);\r\n\r\n            var clickableHeight = rect.height\r\n                - (borderTop + parseFloat(style.borderBottomWidth) || 0)\r\n                - (paddingTop + parseFloat(style.paddingBottom) || 0);\r\n\r\n            var marginW = clickableWidth * 0.2;\r\n            var marginH = clickableHeight * 0.2;\r\n\r\n            var clickX = clickableLeft + marginW + (Math.random() * (clickableWidth - 2 * marginW));\r\n            var clickY = clickableTop + marginH + (Math.random() * (clickableHeight - 2 * marginH));\r\n\r\n            clickX += window.pageXOffset || document.documentElement.scrollLeft;\r\n            clickY += window.pageYOffset || document.documentElement.scrollTop;\r\n\r\n            return [\r\n                clickX, clickY,\r\n                rect.left, rect.top, rect.width, rect.height,\r\n                clickableLeft, clickableTop, clickableWidth, clickableHeight\r\n            ];\r\n        }\r\n    ";
		double[] result = page.EvaluateAsync<double[]>(expression, element).GetAwaiter().GetResult();
		double num = result[0];
		double num2 = result[1];
		string format = "Element rect: left={0}, top={1}, width={2}, height={3}";
		Console.WriteLine(string.Format(format, result[2], result[3], result[4], result[5]));
		string format2 = "Clickable area: left={0}, top={1}, width={2}, height={3}";
		Console.WriteLine(string.Format(format2, result[6], result[7], result[8], result[9]));
		string format3 = "Calculated click: x={0}, y={1}";
		Console.WriteLine(string.Format(format3, num, num2));
		string expression2 = "\r\n        () => {\r\n\r\n            var screenX = 0;\r\n            var screenY = 0;\r\n            var uiHeight = 0;\r\n\r\n            if (typeof window.screenX !== 'undefined') {\r\n                screenX = window.screenX;\r\n                screenY = window.screenY;\r\n            } \r\n            else if (typeof window.screenLeft !== 'undefined') {\r\n                screenX = window.screenLeft;\r\n                screenY = window.screenTop;\r\n            }\r\n\r\n            uiHeight = window.outerHeight - window.innerHeight;\r\n\r\n            return [screenX, screenY, uiHeight];\r\n        }\r\n    ";
		double[] result2 = page.EvaluateAsync<double[]>(expression2).GetAwaiter().GetResult();
		double num3 = result2[0];
		double num4 = result2[1];
		double num5 = result2[2];
		string format4 = "Window: x={0}, y={1}, UI Height: {2}";
		Console.WriteLine(string.Format(format4, num3, num4, num5));
		int num6 = (int)Math.Round(num3 + num);
		int num7 = (int)Math.Round(num4 + num5 + num2);
		string format5 = "Final screen coordinates: {0}, {1}";
		Console.WriteLine(string.Format(format5, num6, num7));
		POINT result3 = default(POINT);
		result3.X = num6;
		result3.Y = num7;
		return result3;
	}

	private void VerifyCoordinates_Play(IPage page, IElementHandle element, int screenX, int screenY)
	{
		string expression = "\r\n    (el, screenX, screenY) => {\r\n        // Remove any existing marker\r\n        var oldMarker = document.getElementById('click-marker');\r\n        if (oldMarker) oldMarker.remove();\r\n\r\n        // Create new marker\r\n        var marker = document.createElement('div');\r\n        marker.id = 'click-marker';\r\n        marker.style.position = 'fixed';\r\n        marker.style.width = '9px';\r\n        marker.style.height = '9px';\r\n        marker.style.backgroundColor = 'blue';\r\n        marker.style.borderRadius = '50%';\r\n        marker.style.boxShadow = '0 0 0 2px white, 0 0 0 4px red';\r\n        marker.style.zIndex = '999999';\r\n        marker.style.pointerEvents = 'none';\r\n\r\n        // Position marker\r\n        var rect = el.getBoundingClientRect();\r\n\r\n        var clickX = screenX - (window.screenX || window.screenLeft || 0);\r\n        var clickY = screenY \r\n                    - (window.screenY || window.screenTop || 0)\r\n                    - (window.outerHeight - window.innerHeight);\r\n\r\n        marker.style.left = (clickX - 2) + 'px';\r\n        marker.style.top = (clickY - 2) + 'px';\r\n\r\n        document.body.appendChild(marker);\r\n\r\n        // Highlight element\r\n        el.style.outline = '2px dashed #00ff00';\r\n        el.style.outlineOffset = '2px';\r\n\r\n        setTimeout(function(){\r\n            if(marker && marker.parentNode){\r\n                marker.parentNode.removeChild(marker);\r\n            }\r\n            el.style.outline = '';\r\n        },2000);\r\n    }\r\n";
		page.EvaluateAsync(expression, new object[3] { element, screenX, screenY }).GetAwaiter().GetResult();
		Thread.Sleep(500);
	}

	private bool IsElementUnderCursor_Play(IPage page, IElementHandle element, int screenX, int screenY)
	{
		try
		{
			string expression = "\r\n            (args) => {\r\n                var el = args.el;\r\n                var screenX = args.screenX;\r\n                var screenY = args.screenY;\r\n\r\n                // Convert screen coordinates to page coordinates\r\n                var pageX = screenX - (window.screenX || window.screenLeft || 0);\r\n\r\n                var pageY = screenY \r\n                    - (window.screenY || window.screenTop || 0)\r\n                    - (window.outerHeight - window.innerHeight);\r\n\r\n                // Get element at point\r\n                var elementAtPoint = document.elementFromPoint(pageX, pageY);\r\n\r\n                // Check if it's our element or a child\r\n                while (elementAtPoint) {\r\n                    if (elementAtPoint === el) return true;\r\n                    elementAtPoint = elementAtPoint.parentElement;\r\n                }\r\n\r\n                return false;\r\n            }\r\n        ";
			return page.EvaluateAsync<bool>(expression, new
			{
				el = element,
				screenX = screenX,
				screenY = screenY
			}).GetAwaiter().GetResult();
		}
		catch
		{
			return false;
		}
	}

	internal void ShowClickDot(IWebDriver driver, int x, int y)
	{
		IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
		string[] array = new string[5];
		string text = "\r\n        var dot = document.createElement('div');\r\n        dot.style.position='fixed';\r\n        dot.style.left='";
		array[0] = text;
		array[1] = x.ToString();
		string text2 = "px';\r\n        dot.style.top='";
		array[2] = text2;
		array[3] = y.ToString();
		string text3 = "px';\r\n        dot.style.width='50px';\r\n        dot.style.height='50px';\r\n        dot.style.background='red';\r\n        dot.style.borderRadius='50%';\r\n        dot.style.zIndex='999999';\r\n        dot.style.pointerEvents='none';\r\n        document.body.appendChild(dot);\r\n\r\n        setTimeout(function(){ dot.remove(); }, 5000);\r\n    ";
		array[4] = text3;
		javaScriptExecutor.ExecuteScript(string.Concat(array));
	}
    internal void HumanLikeClickAndType(IWebDriver driver, By locator, string text)
    {
        try
        {
            IWebElement webElement = driver.FindElement(locator);
            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
            POINT accurateScreenCoordinates = GetAccurateScreenCoordinates(driver, webElement);
            int x = accurateScreenCoordinates.X;
            int y = accurateScreenCoordinates.Y;
            GetCursorPos(out var lpPoint);
            VerifyCoordinates(driver, webElement, x, y);
            Thread.Sleep(rnd.Next(100, 250));
            HumanMoveToPosition(lpPoint.X, lpPoint.Y, x, y);
            GetCursorPos(out var _);
            Thread.Sleep(rnd.Next(100, 250));
            if (IsElementUnderCursor(driver, webElement, x, y))
            {
                mouse_event(2, x, y, 0, 0);
                Thread.Sleep(rnd.Next(30, 100));
                mouse_event(4, x, y, 0, 0);
                Thread.Sleep(rnd.Next(150, 300));
                TypeTextWindowsAPI_new(text);
            }
            else
            {
                string script = "arguments[0].click();";
                javaScriptExecutor.ExecuteScript(script, webElement);
                Thread.Sleep(200);
                TypeTextWindowsAPI_new(text);
            }
        }
        catch
        {
            IWebElement webElement2 = driver.FindElement(locator);
            IJavaScriptExecutor javaScriptExecutor2 = (IJavaScriptExecutor)driver;
            string script2 = "arguments[0].click();";
            javaScriptExecutor2.ExecuteScript(script2, webElement2);
            Thread.Sleep(200);
            TypeTextWindowsAPI_new(text);
        }
    }

    internal void HumanLikeClickAndTypeBRAVE(IWebDriver driver, By locator, string text)
	{
		try
		{
			IWebElement webElement = driver.FindElement(locator);
			IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
			POINT accurateScreenCoordinates = GetElementScreenCoordinatesbbb(driver, webElement);
			int x = accurateScreenCoordinates.X;
			int y = accurateScreenCoordinates.Y;
			GetCursorPos(out var lpPoint);
			VerifyCoordinates(driver, webElement, x, y);
			Thread.Sleep(rnd.Next(100, 250));
			HumanMoveToPosition(lpPoint.X, lpPoint.Y, x, y);
			GetCursorPos(out var _);
			Thread.Sleep(rnd.Next(100, 250));
			if (IsElementUnderCursor(driver, webElement, x, y))
			{
				mouse_event(2, x, y, 0, 0);
				Thread.Sleep(rnd.Next(30, 100));
				mouse_event(4, x, y, 0, 0);
				Thread.Sleep(rnd.Next(150, 300));
				TypeTextWindowsAPI_new(text);
			}
			else
			{
				string script = "arguments[0].click();";
				javaScriptExecutor.ExecuteScript(script, webElement);
				Thread.Sleep(200);
				TypeTextWindowsAPI_new(text);
			}
		}
		catch
		{
			IWebElement webElement2 = driver.FindElement(locator);
			IJavaScriptExecutor javaScriptExecutor2 = (IJavaScriptExecutor)driver;
			string script2 = "arguments[0].click();";
			javaScriptExecutor2.ExecuteScript(script2, webElement2);
			Thread.Sleep(200);
			TypeTextWindowsAPI_new(text);
		}
	}

	private void EnsureCapsLockOff()
	{
		if (Control.IsKeyLocked(System.Windows.Forms.Keys.Capital))
		{
			keybd_event(20, 0, 0, 0);
			Thread.Sleep(40);
			keybd_event(20, 0, 2, 0);
			Thread.Sleep(100);
		}
	}

	internal void TypeTextWindowsAPI_new(string text)
	{
		EnsureCapsLockOff();
		Random random = new Random();
		int num = 0;
		while (num < text.Length)
		{
			short num2 = VkKeyScan(text[num]);
			byte bVk = (byte)((uint)num2 & 0xFFu);
			bool flag = ((byte)((num2 >> 8) & 0xFF) & 1) != 0;
			if (random.Next(0, 10) == 3)
			{
				Thread.Sleep(random.Next(120, 350));
				if (flag)
				{
					goto IL_0064;
				}
			}
			else if (flag)
			{
				goto IL_0064;
			}
			goto IL_006e;
			IL_006e:
			keybd_event(bVk, 0, 0, 0);
			Thread.Sleep(random.Next(30, 80));
			keybd_event(bVk, 0, 2, 0);
			if (flag)
			{
				keybd_event(16, 0, 2, 0);
			}
			Thread.Sleep(random.Next(70, 200));
			num++;
			continue;
			IL_0064:
			keybd_event(16, 0, 0, 0);
			goto IL_006e;
		}
	}

	internal void TypeTextWindowsAPI_0(string text)
	{
		Random random = new Random();
		for (int i = 0; i < text.Length; i++)
		{
			char c = text[i];
			short num = VkKeyScan(c);
			byte bVk = (byte)((uint)num & 0xFFu);
			byte b = (byte)((uint)(num >> 8) & 0xFFu);
			if (random.NextDouble() < 0.08)
			{
				Thread.Sleep(random.Next(180, 450));
			}
			if (((uint)b & (true ? 1u : 0u)) != 0)
			{
				keybd_event(16, 0, 0, 0);
			}
			keybd_event(bVk, 0, 0, 0);
			Thread.Sleep(random.Next(25, 80));
			keybd_event(bVk, 0, 2, 0);
			if (((uint)b & (true ? 1u : 0u)) != 0)
			{
				keybd_event(16, 0, 2, 0);
			}
			int millisecondsTimeout;
			switch (c)
			{
			case ' ':
				millisecondsTimeout = random.Next(160, 320);
				break;
			default:
				millisecondsTimeout = ((i % random.Next(3, 6) == 0) ? random.Next(40, 90) : random.Next(70, 180));
				break;
			case ',':
			case '.':
			case '@':
				millisecondsTimeout = random.Next(200, 420);
				break;
			}
			Thread.Sleep(millisecondsTimeout);
			if (random.NextDouble() < 0.05 && i < text.Length - 1)
			{
				short num2 = VkKeyScan((char)random.Next(97, 122));
				keybd_event((byte)((uint)num2 & 0xFFu), 0, 0, 0);
				Thread.Sleep(random.Next(30, 70));
				keybd_event((byte)((uint)num2 & 0xFFu), 0, 2, 0);
				Thread.Sleep(random.Next(120, 220));
				keybd_event(8, 0, 0, 0);
				Thread.Sleep(40);
				keybd_event(8, 0, 2, 0);
				Thread.Sleep(random.Next(100, 200));
			}
		}
	}

	internal void TypeTextWindowsAPI(string text)
	{
		Random random = new Random();
		for (int i = 0; i < text.Length; i++)
		{
			short num = VkKeyScan(text[i]);
			byte bVk = (byte)((uint)num & 0xFFu);
			byte b = (byte)((uint)(num >> 8) & 0xFFu);
			if (random.Next(0, 10) == 3)
			{
				Thread.Sleep(random.Next(120, 350));
			}
			if (((uint)b & (true ? 1u : 0u)) != 0)
			{
				keybd_event(16, 0, 0, 0);
			}
			keybd_event(bVk, 0, 0, 0);
			Thread.Sleep(random.Next(30, 90));
			keybd_event(bVk, 0, 2, 0);
			if (((uint)b & (true ? 1u : 0u)) != 0)
			{
				keybd_event(16, 0, 2, 0);
			}
			Thread.Sleep(random.Next(60, 220));
			if (random.Next(0, 20) == 5 && i < text.Length - 1)
			{
				short num2 = VkKeyScan((char)random.Next(97, 122));
				keybd_event((byte)((uint)num2 & 0xFFu), 0, 0, 0);
				Thread.Sleep(random.Next(40, 80));
				keybd_event((byte)((uint)num2 & 0xFFu), 0, 2, 0);
				Thread.Sleep(random.Next(120, 250));
				keybd_event(8, 0, 0, 0);
				Thread.Sleep(40);
				keybd_event(8, 0, 2, 0);
				Thread.Sleep(random.Next(100, 200));
			}
		}
	}

	internal void TypeTextWindowsAPI0(string text)
	{
		Random random = new Random();
		for (int i = 0; i < text.Length; i++)
		{
			short num = VkKeyScan(text[i]);
			byte bVk = (byte)((uint)num & 0xFFu);
			byte b = (byte)((uint)(num >> 8) & 0xFFu);
			if (((uint)b & (true ? 1u : 0u)) != 0)
			{
				keybd_event(16, 0, 0, 0);
			}
			keybd_event(bVk, 0, 0, 0);
			Thread.Sleep(random.Next(40, 90));
			keybd_event(bVk, 0, 2, 0);
			if (((uint)b & (true ? 1u : 0u)) != 0)
			{
				keybd_event(16, 0, 2, 0);
			}
			Thread.Sleep(random.Next(80, 200));
		}
	}

	[DllImport("user32.dll")]
	private static extern short VkKeyScan(char ch);

    private POINT GetAccurateScreenCoordinates(IWebDriver driver, IWebElement element)
    {
        IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
        string script = "\r\n            var el = arguments[0];\r\n            \r\n            // Get multiple position measurements\r\n            var rect = el.getBoundingClientRect();\r\n            var offsetRect = el.getBoundingClientRect();\r\n            \r\n            // Also get computed style for borders, padding\r\n            var style = window.getComputedStyle(el);\r\n            var borderLeft = parseFloat(style.borderLeftWidth) || 0;\r\n            var borderTop = parseFloat(style.borderTopWidth) || 0;\r\n            var paddingLeft = parseFloat(style.paddingLeft) || 0;\r\n            var paddingTop = parseFloat(style.paddingTop) || 0;\r\n            \r\n            // Calculate click point (within element, not at exact center)\r\n            var clickableLeft = rect.left + borderLeft + paddingLeft;\r\n            var clickableTop = rect.top + borderTop + paddingTop;\r\n            var clickableWidth = rect.width - (borderLeft + parseFloat(style.borderRightWidth) || 0) - (paddingLeft + parseFloat(style.paddingRight) || 0);\r\n            var clickableHeight = rect.height - (borderTop + parseFloat(style.borderBottomWidth) || 0) - (paddingTop + parseFloat(style.paddingBottom) || 0);\r\n            \r\n            // Random point within clickable area (60-80% from edges)\r\n            var marginW = clickableWidth * 0.2;\r\n            var marginH = clickableHeight * 0.2;\r\n            \r\n            var clickX = clickableLeft + marginW + (Math.random() * (clickableWidth - 2 * marginW));\r\n            var clickY = clickableTop + marginH + (Math.random() * (clickableHeight - 2 * marginH));\r\n            \r\n            // Add scroll offset\r\n            clickX += window.pageXOffset || document.documentElement.scrollLeft;\r\n            clickY += window.pageYOffset || document.documentElement.scrollTop;\r\n            \r\n            return [\r\n                clickX, clickY,\r\n                rect.left, rect.top, rect.width, rect.height,\r\n                clickableLeft, clickableTop, clickableWidth, clickableHeight\r\n            ];\r\n        ";
        ReadOnlyCollection<object> readOnlyCollection = (ReadOnlyCollection<object>)javaScriptExecutor.ExecuteScript(script, element);
        double num = Convert.ToDouble(readOnlyCollection[0]);
        double num2 = Convert.ToDouble(readOnlyCollection[1]);
        string format = "Element rect: left={0}, top={1}, width={2}, height={3}";
        Console.WriteLine(string.Format(format, readOnlyCollection[2], readOnlyCollection[3], readOnlyCollection[4], readOnlyCollection[5]));
        string format2 = "Clickable area: left={0}, top={1}, width={2}, height={3}";
        Console.WriteLine(string.Format(format2, readOnlyCollection[6], readOnlyCollection[7], readOnlyCollection[8], readOnlyCollection[9]));
        string format3 = "Calculated click: x={0}, y={1}";
        Console.WriteLine(string.Format(format3, num, num2));
        string script2 = "\r\n            // Get window position - different browsers\r\n            var screenX = 0;\r\n            var screenY = 0;\r\n            var uiHeight = 0;\r\n            \r\n            if (typeof window.screenX !== 'undefined') {\r\n                screenX = window.screenX;\r\n                screenY = window.screenY;\r\n            } else if (typeof window.screenLeft !== 'undefined') {\r\n                screenX = window.screenLeft;\r\n                screenY = window.screenTop;\r\n            }\r\n            \r\n            // Calculate browser UI height (address bar, tabs, etc.)\r\n            uiHeight = window.outerHeight - window.innerHeight;\r\n            \r\n            // For some browsers, need to adjust for taskbar/UI\r\n            if (window.chrome && chrome.app && chrome.app.window) {\r\n                // Chrome app specific\r\n            }\r\n            \r\n            return [screenX, screenY, uiHeight];\r\n        ";
        ReadOnlyCollection<object> readOnlyCollection2 = (ReadOnlyCollection<object>)javaScriptExecutor.ExecuteScript(script2);
        double num3 = Convert.ToDouble(readOnlyCollection2[0]);
        double num4 = Convert.ToDouble(readOnlyCollection2[1]);
        double num5 = Convert.ToDouble(readOnlyCollection2[2]);
        int num6 = (int)Math.Round(num3 + num);
        int num7 = (int)Math.Round(num4 + num5 + num2);
        string format4 = "Final screen coordinates: {0}, {1}";
        Console.WriteLine(string.Format(format4, num6, num7));
        POINT result = default(POINT);
        result.X = num6;
        result.Y = num7;
        return result;
    }
    private POINT GetElementScreenCoordinatesbbb(IWebDriver driver, IWebElement element)
    {
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

        var result = (ReadOnlyCollection<object>)js.ExecuteScript(@"
        var r = arguments[0].getBoundingClientRect();

        return [
            r.left + r.width / 2,
            r.top + r.height / 2,
            window.screenX,
            window.screenY,
            window.outerHeight,
            window.innerHeight,
            window.devicePixelRatio
        ];
    ", element);

        double elementX = Convert.ToDouble(result[0]);
        double elementY = Convert.ToDouble(result[1]);

        double screenX = Convert.ToDouble(result[2]);
        double screenY = Convert.ToDouble(result[3]);

        double outerHeight = Convert.ToDouble(result[4]);
        double innerHeight = Convert.ToDouble(result[5]);

        double dpr = Convert.ToDouble(result[6]);

        double browserTop = outerHeight - innerHeight;

        POINT pt;
        pt.X = (int)Math.Round((screenX + elementX) * dpr);
        pt.Y = (int)Math.Round((screenY + browserTop + elementY) * dpr);

        Console.WriteLine($"DPR: {dpr}");
        Console.WriteLine($"Screen: {screenX},{screenY}");
        Console.WriteLine($"BrowserTop: {browserTop}");
        Console.WriteLine($"Final: {pt.X},{pt.Y}");

        return pt;
    }

    private POINT GetAccurateScreenCoordinatesBrave(IWebDriver driver, IWebElement element)
	{
		IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
		string script = "\r\n            var el = arguments[0];\r\n            \r\n            // Get multiple position measurements\r\n            var rect = el.getBoundingClientRect();\r\n            var offsetRect = el.getBoundingClientRect();\r\n            \r\n            // Also get computed style for borders, padding\r\n            var style = window.getComputedStyle(el);\r\n            var borderLeft = parseFloat(style.borderLeftWidth) || 0;\r\n            var borderTop = parseFloat(style.borderTopWidth) || 0;\r\n            var paddingLeft = parseFloat(style.paddingLeft) || 0;\r\n            var paddingTop = parseFloat(style.paddingTop) || 0;\r\n            \r\n            // Calculate click point (within element, not at exact center)\r\n            var clickableLeft = rect.left + borderLeft + paddingLeft;\r\n            var clickableTop = rect.top + borderTop + paddingTop;\r\n            var clickableWidth = rect.width - (borderLeft + parseFloat(style.borderRightWidth) || 0) - (paddingLeft + parseFloat(style.paddingRight) || 0);\r\n            var clickableHeight = rect.height - (borderTop + parseFloat(style.borderBottomWidth) || 0) - (paddingTop + parseFloat(style.paddingBottom) || 0);\r\n            \r\n            // Random point within clickable area (60-80% from edges)\r\n            var marginW = clickableWidth * 0.2;\r\n            var marginH = clickableHeight * 0.2;\r\n            \r\n            var clickX = clickableLeft + marginW + (Math.random() * (clickableWidth - 2 * marginW));\r\n            var clickY = clickableTop + marginH + (Math.random() * (clickableHeight - 2 * marginH));\r\n            \r\n            // Add scroll offset\r\n            clickX += window.pageXOffset || document.documentElement.scrollLeft;\r\n            clickY += window.pageYOffset || document.documentElement.scrollTop;\r\n            \r\n            return [\r\n                clickX, clickY,\r\n                rect.left, rect.top, rect.width, rect.height,\r\n                clickableLeft, clickableTop, clickableWidth, clickableHeight\r\n            ];\r\n        ";
		ReadOnlyCollection<object> readOnlyCollection = (ReadOnlyCollection<object>)javaScriptExecutor.ExecuteScript(script, element);
		double num = Convert.ToDouble(readOnlyCollection[0]);
		double num2 = Convert.ToDouble(readOnlyCollection[1]);
		string format = "Element rect: left={0}, top={1}, width={2}, height={3}";
		Console.WriteLine(string.Format(format, readOnlyCollection[2], readOnlyCollection[3], readOnlyCollection[4], readOnlyCollection[5]));
		string format2 = "Clickable area: left={0}, top={1}, width={2}, height={3}";
		Console.WriteLine(string.Format(format2, readOnlyCollection[6], readOnlyCollection[7], readOnlyCollection[8], readOnlyCollection[9]));
		string format3 = "Calculated click: x={0}, y={1}";
		Console.WriteLine(string.Format(format3, num, num2));
		string script2 = "\r\n            // Get window position - different browsers\r\n            var screenX = 0;\r\n            var screenY = 0;\r\n            var uiHeight = 0;\r\n            \r\n            if (typeof window.screenX !== 'undefined') {\r\n                screenX = window.screenX;\r\n                screenY = window.screenY;\r\n            } else if (typeof window.screenLeft !== 'undefined') {\r\n                screenX = window.screenLeft;\r\n                screenY = window.screenTop;\r\n            }\r\n            \r\n            // Calculate browser UI height (address bar, tabs, etc.)\r\n            uiHeight = window.outerHeight - window.innerHeight;\r\n            \r\n            // For some browsers, need to adjust for taskbar/UI\r\n            if (window.chrome && chrome.app && chrome.app.window) {\r\n                // Chrome app specific\r\n            }\r\n            \r\n            return [screenX, screenY, uiHeight];\r\n        ";
		ReadOnlyCollection<object> readOnlyCollection2 = (ReadOnlyCollection<object>)javaScriptExecutor.ExecuteScript(script2);
		double num3 = Convert.ToDouble(readOnlyCollection2[0]);
		double num4 = Convert.ToDouble(readOnlyCollection2[1]);
		double num5 = Convert.ToDouble(readOnlyCollection2[2]);
		int num6 = (int)Math.Round(num3 + num);
		int num7 = (int)Math.Round(num4 + num5 + num2);
		string format4 = "Final screen coordinates: {0}, {1}";
		Console.WriteLine(string.Format(format4, num6, num7));
		POINT result = default(POINT);
		result.X = num6;
		result.Y = num7;
		return result;
	}

	private void VerifyCoordinates(IWebDriver driver, IWebElement element, int screenX, int screenY)
	{
		IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
		string script = "\r\n            // Remove any existing marker\r\n            var oldMarker = document.getElementById('click-marker');\r\n            if (oldMarker) oldMarker.remove();\r\n            \r\n            // Create new marker\r\n            var marker = document.createElement('div');\r\n            marker.id = 'click-marker';\r\n            marker.style.position = 'fixed';\r\n            marker.style.width = '9px';\r\n            marker.style.height = '9px';\r\n            marker.style.backgroundColor = 'blue';\r\n            marker.style.borderRadius = '50%';\r\n            marker.style.boxShadow = '0 0 0 2px white, 0 0 0 4px red';\r\n            marker.style.zIndex = '999999';\r\n            marker.style.pointerEvents = 'none';\r\n            \r\n            // Position marker\r\n            var rect = arguments[0].getBoundingClientRect();\r\n            var clickX = arguments[1] - (window.screenX || window.screenLeft || 0);\r\n            var clickY = arguments[2] - (window.screenY || window.screenTop || 0) - (window.outerHeight - window.innerHeight);\r\n            \r\n            marker.style.left = (clickX - 2) + 'px';\r\n            marker.style.top = (clickY - 2) + 'px';\r\n            \r\n            document.body.appendChild(marker);\r\n            \r\n            // Also highlight the element\r\n            arguments[0].style.outline = '2px dashed #00ff00';\r\n            arguments[0].style.outlineOffset = '2px';\r\n            \r\n            // Remove after 2 seconds\r\n            setTimeout(function() {\r\n                if (marker && marker.parentNode) {\r\n                    marker.parentNode.removeChild(marker);\r\n                }\r\n                arguments[0].style.outline = '';\r\n            }, 2000, arguments[0]);\r\n        ";
		javaScriptExecutor.ExecuteScript(script, element, screenX, screenY);
		Thread.Sleep(500);
	}
    internal void HumanMoveToPosition1(int startX, int startY, int endX, int endY)
    {
        Random rnd = new Random();
        int steps = rnd.Next(25, 50);

        double cp1X = startX + rnd.Next(-120, 120);
        double cp1Y = startY + rnd.Next(-120, 120);

        double cp2X = endX + rnd.Next(-120, 120);
        double cp2Y = endY + rnd.Next(-120, 120);

        for (int i = 0; i <= steps; i++)
        {
            double t = (double)i / steps;

            double x = Math.Pow(1 - t, 3) * startX +
                       3 * Math.Pow(1 - t, 2) * t * cp1X +
                       3 * (1 - t) * Math.Pow(t, 2) * cp2X +
                       Math.Pow(t, 3) * endX;

            double y = Math.Pow(1 - t, 3) * startY +
                       3 * Math.Pow(1 - t, 2) * t * cp1Y +
                       3 * (1 - t) * Math.Pow(t, 2) * cp2Y +
                       Math.Pow(t, 3) * endY;

            SetCursorPos((int)x, (int)y);

            Thread.Sleep(rnd.Next(5, 15));
        }
    }
    private void HumanMoveToPosition(int startX, int startY, int targetX, int targetY)
	{
		string format = "Moving from ({0},{1}) to ({2},{3})";
		Console.WriteLine(string.Format(format, startX, startY, targetX, targetY));
		int num = (int)Math.Sqrt(Math.Pow(targetX - startX, 2.0) + Math.Pow(targetY - startY, 2.0));
		string format2 = "Distance: {0}px";
		Console.WriteLine(string.Format(format2, num));
		if (num < 50)
		{
			SmallDistanceMove(startX, startY, targetX, targetY);
		}
		else if (num >= 200)
		{
			LargeDistanceMove(startX, startY, targetX, targetY);
		}
		else
		{
			MediumDistanceMove(startX, startY, targetX, targetY);
		}
	}

	private void SmallDistanceMove(int startX, int startY, int targetX, int targetY)
	{
		int num = 5 + rnd.Next(2, 6);
		for (int i = 1; i <= num; i++)
		{
			double num2 = (double)i / (double)num;
			num2 = num2 * num2 * (3.0 - 2.0 * num2);
			int x = startX + (int)((double)(targetX - startX) * num2);
			int y = startY + (int)((double)(targetY - startY) * num2);
			SetCursorPos(x, y);
			Thread.Sleep(20 + rnd.Next(0, 10));
		}
		SetCursorPos(targetX, targetY);
	}

	private void MediumDistanceMove(int startX, int startY, int targetX, int targetY)
	{
		int num = startX + (int)((double)(targetX - startX) * 0.8);
		int num2 = startY + (int)((double)(targetY - startY) * 0.8);
		num += rnd.Next(-15, 16);
		num2 += rnd.Next(-15, 16);
		int num3 = 6 + rnd.Next(2, 5);
		for (int i = 1; i <= num3; i++)
		{
			double x = (double)i / (double)num3;
			x = Math.Pow(x, 1.5);
			int x2 = startX + (int)((double)(num - startX) * x);
			int y = startY + (int)((double)(num2 - startY) * x);
			SetCursorPos(x2, y);
			Thread.Sleep(12 + rnd.Next(0, 8));
		}
		Thread.Sleep(rnd.Next(50, 150));
		GetCursorPos(out var lpPoint);
		int num4 = 8 + rnd.Next(2, 6);
		for (int j = 1; j <= num4; j++)
		{
			double num5 = (double)j / (double)num4;
			num5 = 1.0 - Math.Pow(1.0 - num5, 2.0);
			int x3 = lpPoint.X + (int)((double)(targetX - lpPoint.X) * num5);
			int y2 = lpPoint.Y + (int)((double)(targetY - lpPoint.Y) * num5);
			SetCursorPos(x3, y2);
			Thread.Sleep(25 + rnd.Next(0, 10));
		}
		SetCursorPos(targetX, targetY);
	}

	private void LargeDistanceMove(int startX, int startY, int targetX, int targetY)
	{
		int num = targetX + rnd.Next(-40, 41);
		int num2 = targetY + rnd.Next(-40, 41);
		int num3 = 8 + rnd.Next(2, 6);
		for (int i = 1; i <= num3; i++)
		{
			double num4 = (double)i / (double)num3;
			num4 *= num4;
			int num5 = startX + (int)((double)(num - startX) * num4);
			int num6 = startY + (int)((double)(num2 - startY) * num4);
			int x = num5 + rnd.Next(-8, 9);
			num6 += rnd.Next(-8, 9);
			SetCursorPos(x, num6);
			Thread.Sleep(10);
		}
		Thread.Sleep(rnd.Next(100, 250));
		for (int j = 0; j < rnd.Next(2, 5); j++)
		{
			GetCursorPos(out var lpPoint);
			SetCursorPos(lpPoint.X + rnd.Next(-20, 21), lpPoint.Y + rnd.Next(-20, 21));
			Thread.Sleep(rnd.Next(40, 100));
		}
		GetCursorPos(out var lpPoint2);
		int num7 = 12 + rnd.Next(3, 8);
		for (int k = 1; k <= num7; k++)
		{
			double num8 = (double)k / (double)num7;
			num8 = ((num8 < 0.5) ? (2.0 * num8 * num8) : (1.0 - Math.Pow(-2.0 * num8 + 2.0, 2.0) / 2.0));
			int x2 = lpPoint2.X + (int)((double)(targetX - lpPoint2.X) * num8);
			int y = lpPoint2.Y + (int)((double)(targetY - lpPoint2.Y) * num8);
			SetCursorPos(x2, y);
			Thread.Sleep(30 + rnd.Next(0, 15));
		}
		SetCursorPos(targetX, targetY);
	}

	private bool IsElementUnderCursor(IWebDriver driver, IWebElement element, int screenX, int screenY)
	{
		IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
		try
		{
			string script = "\r\n                var el = arguments[0];\r\n                var screenX = arguments[1];\r\n                var screenY = arguments[2];\r\n                \r\n                // Convert screen coordinates to page coordinates\r\n                var pageX = screenX - (window.screenX || window.screenLeft || 0);\r\n                var pageY = screenY - (window.screenY || window.screenTop || 0) - (window.outerHeight - window.innerHeight);\r\n                \r\n                // Get element at point\r\n                var elementAtPoint = document.elementFromPoint(pageX, pageY);\r\n                \r\n                // Check if it's our element or a child\r\n                while (elementAtPoint) {\r\n                    if (elementAtPoint === el) return true;\r\n                    elementAtPoint = elementAtPoint.parentElement;\r\n                }\r\n                \r\n                return false;\r\n            ";
			return (bool)javaScriptExecutor.ExecuteScript(script, element, screenX, screenY);
		}
		catch
		{
			return false;
		}
	}

	internal void SimpleAccurateClick(IWebDriver driver, By locator)
	{
		IWebElement webElement = driver.FindElement(locator);
		IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
		string script = "\r\n            var el = arguments[0];\r\n            var rect = el.getBoundingClientRect();\r\n            \r\n            // Simple center point\r\n            var centerX = rect.left + (rect.width / 2);\r\n            var centerY = rect.top + (rect.height / 2);\r\n            \r\n            // Add scroll\r\n            centerX += window.pageXOffset;\r\n            centerY += window.pageYOffset;\r\n            \r\n            return [centerX, centerY];\r\n        ";
		ReadOnlyCollection<object> readOnlyCollection = (ReadOnlyCollection<object>)javaScriptExecutor.ExecuteScript(script, webElement);
		double num = Convert.ToDouble(readOnlyCollection[0]);
		double num2 = Convert.ToDouble(readOnlyCollection[1]);
		string script2 = "\r\n            return [\r\n                window.screenX || window.screenLeft || 0,\r\n                window.screenY || window.screenTop || 0,\r\n                window.outerHeight - window.innerHeight\r\n            ];\r\n        ";
		ReadOnlyCollection<object> readOnlyCollection2 = (ReadOnlyCollection<object>)javaScriptExecutor.ExecuteScript(script2);
		double num3 = Convert.ToDouble(readOnlyCollection2[0]);
		double num4 = Convert.ToDouble(readOnlyCollection2[1]);
		double num5 = Convert.ToDouble(readOnlyCollection2[2]);
		int num6 = (int)(num3 + num);
		int num7 = (int)(num4 + num5 + num2);
		GetCursorPos(out var _);
		SetCursorPos(num6, num7);
		Thread.Sleep(300);
		mouse_event(2, num6, num7, 0, 0);
		Thread.Sleep(50);
		mouse_event(4, num6, num7, 0, 0);
	}

	internal void TestButtonClick(IWebDriver driver, By locator)
	{
		IWebElement webElement = driver.FindElement(locator);
		IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
		string value = "=== TESTING BUTTON CLICK ===";
		Console.WriteLine(value);
		string value2 = "Method 1: JavaScript click";
		Console.WriteLine(value2);
		string script = "arguments[0].click();";
		javaScriptExecutor.ExecuteScript(script, webElement);
		Thread.Sleep(1000);
		string url = driver.Url;
		Thread.Sleep(2000);
		string url2 = driver.Url;
		if (url != url2)
		{
			string value3 = "✓ JavaScript click worked!";
			Console.WriteLine(value3);
			driver.Navigate().Back();
			Thread.Sleep(2000);
		}
		else
		{
			string value4 = "✗ JavaScript click failed";
			Console.WriteLine(value4);
		}
		string value5 = "\nMethod 2: Accurate mouse click";
		Console.WriteLine(value5);
		HumanLikeClickWithRandomApproach(driver, locator);
		Thread.Sleep(2000);
		url2 = driver.Url;
		if (url != url2)
		{
			string value6 = "✓ Mouse click worked!";
			Console.WriteLine(value6);
		}
		else
		{
			string value7 = "✗ Mouse click failed";
			Console.WriteLine(value7);
		}
	}
}
