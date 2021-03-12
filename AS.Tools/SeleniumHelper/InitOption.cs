using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Tools.SeleniumHelper
{
    /// <summary>
    /// Simple init option (Support Chrome | Firefox)
    /// </summary>
    public class InitOptions
    {
        /// <summary>
        /// Profile directory
        /// </summary>
        public string ProfileDirectory { get; set; }
        
        /// <summary>
        /// User data dir name
        /// </summary>
        public string UserDataDir { get; set; }

        /// <summary>
        /// Binary location (to chrome.exe or firefox.exe). If null, use default
        /// </summary>
        public string BinaryLocation { get; set; }

        /// <summary>
        /// Use headless
        /// </summary>
        public bool Headless { get; set; }

        /// <summary>
        /// Load image
        /// </summary>
        public bool IsLoadImage { get; set; }

        /// <summary>
        /// Window position X
        /// </summary>
        public int WinPosX { get; set; }

        /// <summary>
        /// Window position Y
        /// </summary>
        public int WinPosY { get; set; }

        /// <summary>
        /// Init default value
        /// </summary>
        public InitOptions()
        {
            this.WinPosX = this.WinPosY = -1;
            this.IsLoadImage = true;
        }

        /// <summary>
        /// Parse to chrome option
        /// </summary>
        /// <returns></returns>
        public ChromeOptions ToChromeOptions()
        {
            ChromeOptions options = new ChromeOptions();

            // User data dir
            if (!String.IsNullOrEmpty(this.ProfileDirectory) && Directory.Exists(this.UserDataDir))
            {
                options.AddArgument("--user-data-dir=" + this.UserDataDir);
                options.AddArguments("--profile-directory=" + this.ProfileDirectory);
            }
            
            if (File.Exists(this.BinaryLocation))
                options.BinaryLocation = this.BinaryLocation;

            // Window pos
            if (this.WinPosX != -1 && this.WinPosY != -1)
                options.AddArgument($"--window-position={this.WinPosX},{this.WinPosY}");

            // Hide window
            if (this.Headless)
                options.AddArgument("headless");

            // Disable load image (1 | 2)
            if (!this.IsLoadImage)
                options.AddUserProfilePreference("profile.managed_default_content_settings.images", 2);

            // Hide "Chrome being controll ..." message - real browse
            options.AddArgument("--disable-blink-features");
            options.AddArgument("--disable-blink-features=AutomationControlled");

            options.AddAdditionalCapability("useAutomationExtension", false);
            options.AddExcludedArgument("enable-automation");

            return options;
        }
    }
}
