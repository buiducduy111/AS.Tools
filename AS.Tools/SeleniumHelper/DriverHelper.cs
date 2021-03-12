using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace AS.Tools.SeleniumHelper
{
    /// <summary>
    /// Driver helper for selenium fast code
    /// </summary>
    public class DriverHelper
    {
        IWebDriver _driver;

        /// <summary>
        /// Init with IWebdriver. You can init by static method: DriverHelper.InitChromeDriver or InitFirefoxDriver
        /// </summary>
        /// <param name="driver"></param>
        public DriverHelper(IWebDriver driver)
        {
            _driver = driver;
        }

        /// <summary>
        /// Go to url util success
        /// </summary>
        /// <param name="url"></param>
        public void GoToUrl(string url)
        {
            while (true)
            {
                try
                {
                    _driver.Navigate().GoToUrl(url);
                    return;
                }
                catch
                {
                    _driver.Navigate().Refresh();
                    Thread.Sleep(3000);
                }
            }
        }

        /// <summary>
        /// Wait thread util the element appears
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="timeOut"></param>
        /// <returns>Return true if success, false if time-out</returns>
        public bool WaitElementByXpath(string xpath, int timeOut = 60)
        {
            int time = 0;

            while (_driver.FindElements(By.XPath(xpath)).Count == 0)
            {
                Thread.Sleep(1000);

                time++;
                if (time > timeOut)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check one of the XPaths list appears
        /// </summary>
        /// <param name="elementsXpath"></param>
        /// <param name="timeOut"></param>
        /// <returns>Return index of xpath list if success, -1 if time out</returns>
        public int HasElement(List<string> elementsXpath, int timeOut = 20)
        {
            int time = 0;

            while (true)
            {
                for (int i = 0; i < elementsXpath.Count; i++)
                {
                    if (_driver.FindElements(By.XPath(elementsXpath[i])).Count > 0)
                        return i;
                }

                Thread.Sleep(1000);

                time++;
                if (time > timeOut)
                    return -1;
            }
        }

        /// <summary>
        /// Scroll to Element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="offset"></param>
        public void ScrollToElement(IWebElement element, int offset = 0)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("window.scrollTo({top: " + (element.Location.Y + offset) + ",behavior: 'smooth'})");
        }

        /// <summary>
        /// Scroll smooth to bottom
        /// </summary>
        public void ScrollToBottom()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("window.scrollTo({top: document.body.scrollHeight ,behavior: 'smooth'});");
        }

        /// <summary>
        /// Scroll to Y pos
        /// </summary>
        /// <param name="y"></param>
        public void ScrollToPosition(int y)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("window.scrollTo({top: " + y + ",behavior: 'smooth'})");
        }

        /// <summary>
        /// Open a new tab
        /// </summary>
        /// <param name="url"></param>
        /// <param name="isSwitchToNewTab"></param>
        public void OpenNewTab(string url, bool isSwitchToNewTab = true)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript($"window.open('{url}')");
            Thread.Sleep(1000);

            if (_driver.WindowHandles.Count > 1 && isSwitchToNewTab)
                _driver.SwitchTo().Window(_driver.WindowHandles[_driver.WindowHandles.Count - 1]);
        }

        /// <summary>
        /// Close current tab
        /// </summary>
        public void CloseCurrentTab()
        {
            try
            {
                _driver.Close();
                _driver.SwitchTo().Window(_driver.WindowHandles[0]);
            }
            catch { }
        }

        /// <summary>
        /// Click to Element by Javascript. Sometime, chrome cannot click by default
        /// </summary>
        /// <param name="element"></param>
        public void ClickByJs(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("arguments[0].click();", element);
        }

        #region Static init / close driver
        /// <summary>
        /// Init chrome session
        /// </summary>
        /// <param name="initOptions"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ChromeDriver InitChromeDriver(InitOptions initOptions, out string msg)
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();

            // Hide cmd window
            service.HideCommandPromptWindow = true;

            // Options
            ChromeOptions options = initOptions.ToChromeOptions();

            try
            {
                msg = "Khởi tạo thành công";
                return new ChromeDriver(service, options);
            }
            catch (Exception ex)
            {
                msg = "Khởi tạo thất bại: " + ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Init firefox driver. Return null if init fail
        /// </summary>
        /// <param name="profilePath"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static FirefoxDriver InitFirefoxDriver(string profilePath, out string msg)
        {
            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            FirefoxOptions options = new FirefoxOptions();
            options.SetPreference("dom.webdriver.enabled", false);

            if (!String.IsNullOrEmpty(profilePath))
            {
                FirefoxProfile profile = new FirefoxProfile(profilePath);
                options.Profile = profile;
            }

            try
            {
                msg = "Khởi tạo thành công";
                return new FirefoxDriver(service, options);
            }
            catch (Exception ex)
            {
                msg = "Khởi tạo thất bại: " + ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Close all chrome process
        /// </summary>
        public static void CloseAllChrome()
        {
            Process[] processes = Process.GetProcessesByName("chromedriver");
            foreach (Process p in processes)
            {
                try
                {
                    p.Kill();
                }
                catch { }
            }

            processes = Process.GetProcessesByName("chrome");
            foreach (Process p in processes)
            {
                try
                {
                    p.Kill();
                }
                catch { }
            }
        }

        /// <summary>
        /// Close all firefox process
        /// </summary>
        public static void CloseAllFirefox()
        {
            Process[] processes = Process.GetProcessesByName("geckodriver");
            foreach (Process p in processes)
            {
                try
                {
                    p.Kill();
                }
                catch { }
            }

            processes = Process.GetProcessesByName("firefox");
            foreach (Process p in processes)
            {
                try
                {
                    p.Kill();
                }
                catch { }
            }
        }
        #endregion
    }
}
