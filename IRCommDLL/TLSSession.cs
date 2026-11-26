using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace IRCommDLL;

internal class TLSSession
{
        private readonly RequestPayload sessionPayload;

        public TLSSession(Dictionary<string, string> headers = null, string TlsClientIdentifier = "chrome_133", int TimeoutSeconds = 60, bool FollowRedirects = false, string proxy = null)
        {
                sessionPayload = new RequestPayload
                {
                        TlsClientIdentifier = TlsClientIdentifier,
                        FollowRedirects = FollowRedirects,
                        TimeoutSeconds = TimeoutSeconds,
                        Headers = (headers ?? new Dictionary<string, string>()),
                        HeaderOrder = ((headers != null) ? new List<string>(headers.Keys) : new List<string>()),
                        ProxyUrl = (proxy ?? ""),
                        CertificatePinningHosts = new Dictionary<string, string>()
                };
        }

        public RequestResult Get(string url, Dictionary<string, string> additionalHeaders = null)
        {
                return MakeRequest("GET", url, MergeHeaders(sessionPayload.Headers, additionalHeaders), "");
        }

        public RequestResult Post(string url, Dictionary<string, string> additionalHeaders = null, string body = "")
        {
                return MakeRequest("POST", url, MergeHeaders(sessionPayload.Headers, additionalHeaders), body);
        }

        public RequestResult Patch(string url, Dictionary<string, string> additionalHeaders = null, string body = "")
        {
                return MakeRequest("PATCH", url, MergeHeaders(sessionPayload.Headers, additionalHeaders), body);
        }

        public RequestResult Put(string url, Dictionary<string, string> additionalHeaders = null, string body = "")
        {
                return MakeRequest("PUT", url, MergeHeaders(sessionPayload.Headers, additionalHeaders), body);
        }

        public RequestResult Delete(string url, Dictionary<string, string> additionalHeaders = null)
        {
                return MakeRequest("DELETE", url, MergeHeaders(sessionPayload.Headers, additionalHeaders), "");
        }

        private RequestResult MakeRequest(string method, string url, Dictionary<string, string> headers, string body)
        {
                RequestResult result = new RequestResult
                {
                        Id = Guid.NewGuid().ToString(),
                        Target = url,
                        Headers = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase),
                        Body = "",
                        Status = 0,
                        UsedProtocol = "http/1.1"
                };

                try
                {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        ServicePointManager.ServerCertificateValidationCallback = (s, c, ch, e) => true;
                        ServicePointManager.DefaultConnectionLimit = 200;

                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                        req.Method = method;
                        req.Timeout = Math.Max(5, sessionPayload.TimeoutSeconds) * 1000;
                        req.ReadWriteTimeout = req.Timeout;
                        req.AllowAutoRedirect = sessionPayload.FollowRedirects;
                        req.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                        req.KeepAlive = true;
                        req.Proxy = BuildProxy(sessionPayload.ProxyUrl);
            CookieContainer cookieJar = new CookieContainer();
            req.CookieContainer = cookieJar;

            if (headers != null)
                        {
                                foreach (KeyValuePair<string, string> h in headers)
                                {
                                        if (string.IsNullOrEmpty(h.Key)) continue;
                                        try
                                        {
                                                req.Headers[h.Key] = h.Value;
                                        }
                                        catch
                                        {
                        switch (h.Key.ToLowerInvariant())
                        {
                            case "user-agent": req.UserAgent = h.Value; break;
                            case "accept": req.Accept = h.Value; break;
                            case "referer": req.Referer = h.Value; break;
                            case "content-type": req.ContentType = h.Value; break;
                            case "host": req.Host = h.Value; break;
                            case "connection": req.KeepAlive = h.Value.ToLowerInvariant().Contains("keep-alive"); break;
                            case "cookie":
                                foreach (string ck in h.Value.Split(';'))
                                {
                                    string pair = ck.Trim();
                                    int eqi = pair.IndexOf('=');
                                    if (eqi > 0)
                                    {
                                        try { cookieJar.Add(new Uri(url), new Cookie(pair.Substring(0, eqi).Trim(), pair.Substring(eqi + 1).Trim())); }
                                        catch { }
                                    }
                                }
                                break;
                            case "accept-encoding": break;
                            case "content-length": break;
                            case "expect": break;
                            case "date": break;
                            case "if-modified-since": break;
                        }
                    }
                                }
                        }

                        if (!string.IsNullOrEmpty(body) && method != "GET" && method != "HEAD")
                        {
                                byte[] data = Encoding.UTF8.GetBytes(body);
                                if (string.IsNullOrEmpty(req.ContentType)) req.ContentType = "application/json";
                                req.ContentLength = data.Length;
                                using (Stream s = req.GetRequestStream())
                                {
                                        s.Write(data, 0, data.Length);
                                }
                        }

                        using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                        {
                                result.Status = (int)resp.StatusCode;
                                foreach (string key in resp.Headers.AllKeys)
                                {
                                        try { result.Headers[key] = new List<string>(resp.Headers.GetValues(key)); }
                                        catch { }
                                }
                                using (Stream rs = resp.GetResponseStream())
                                using (StreamReader sr = new StreamReader(rs, Encoding.UTF8))
                                {
                                        result.Body = sr.ReadToEnd();
                                }
                                result.UsedProtocol = resp.ProtocolVersion.ToString();
                        }
                }
                catch (WebException wex)
                {
                        try
                        {
                                if (wex.Response is HttpWebResponse er)
                                {
                                        result.Status = (int)er.StatusCode;
                                        using (Stream es = er.GetResponseStream())
                                        using (StreamReader esr = new StreamReader(es, Encoding.UTF8))
                                        {
                                                result.Body = esr.ReadToEnd();
                                        }
                                }
                                else
                                {
                                        result.Body = wex.Message;
                                }
                        }
                        catch
                        {
                                result.Body = wex.Message;
                        }
                }
                catch (Exception ex)
                {
                        result.Body = ex.Message;
                }

                return result;
        }

        private static IWebProxy BuildProxy(string proxyUrl)
        {
                if (string.IsNullOrWhiteSpace(proxyUrl)) return null;
                try
                {
                        if (!proxyUrl.Contains("://")) proxyUrl = "http://" + proxyUrl;
                        Uri uri = new Uri(proxyUrl);
                        WebProxy p = new WebProxy(uri.Host, uri.Port) { BypassProxyOnLocal = true };
                        if (!string.IsNullOrEmpty(uri.UserInfo))
                        {
                                string[] parts = uri.UserInfo.Split(':');
                                p.Credentials = new NetworkCredential(parts[0], parts.Length > 1 ? parts[1] : "");
                        }
                        return p;
                }
                catch
                {
                        return null;
                }
        }

        private Dictionary<string, string> MergeHeaders(Dictionary<string, string> originalHeaders, Dictionary<string, string> additionalHeaders)
        {
                if (additionalHeaders != null)
                {
                        foreach (KeyValuePair<string, string> additionalHeader in additionalHeaders)
                        {
                                originalHeaders[additionalHeader.Key] = additionalHeader.Value;
                        }
                        return originalHeaders;
                }
                return originalHeaders;
        }
}