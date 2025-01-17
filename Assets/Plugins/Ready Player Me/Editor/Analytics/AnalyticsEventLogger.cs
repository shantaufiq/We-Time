﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Analytics
{
    public class AnalyticsEventLogger : IAnalyticsEventLogger
    {
        private readonly AmplitudeEventLogger amplitudeEventLogger;

        private bool isEnabled;

        public AnalyticsEventLogger(bool isEnabled)
        {
            amplitudeEventLogger = new AmplitudeEventLogger();
            this.isEnabled = isEnabled;
        }

        [Obsolete]
        public void Enable()
        {
            isEnabled = true;
            if (!amplitudeEventLogger.IsSessionIdSet())
            {
                GenerateSessionId();
            }
            ToggleAnalytics(true);
        }

        [Obsolete]
        public void Disable()
        {
            ToggleAnalytics(false);
            isEnabled = false;
            amplitudeEventLogger.SetSessionId(0);
        }

        [Obsolete]
        public void IdentifyUser()
        {
            if (!isEnabled) return;
            if (!amplitudeEventLogger.IsSessionIdSet())
            {
                GenerateSessionId();
            }
            amplitudeEventLogger.SetUserProperties();
        }

        [Obsolete]
        public void LogOpenProject()
        {
            if (!isEnabled) return;
            GenerateSessionId();
            amplitudeEventLogger.LogEvent(Constants.EventName.OPEN_PROJECT);
        }

        [Obsolete]
        public void LogCloseProject()
        {
            if (!isEnabled) return;
            amplitudeEventLogger.LogEvent(Constants.EventName.CLOSE_PROJECT);
        }

        [Obsolete]
        public void LogOpenDocumentation(string target)
        {
            if (!isEnabled) return;
            amplitudeEventLogger.LogEvent(Constants.EventName.OPEN_DOCUMENTATION, new Dictionary<string, object>()
            {
                { Constants.Properties.TARGET, target }
            });
        }

        [Obsolete]
        public void LogOpenFaq(string target)
        {
            if (!isEnabled) return;
            amplitudeEventLogger.LogEvent(Constants.EventName.OPEN_FAQ, new Dictionary<string, object>()
            {
                { Constants.Properties.TARGET, target }
            });
        }

        [Obsolete]
        public void LogOpenDiscord(string target)
        {
            if (!isEnabled) return;
            amplitudeEventLogger.LogEvent(Constants.EventName.OPEN_DISCORD, new Dictionary<string, object>()
            {
                { Constants.Properties.TARGET, target }
            });
        }

        [Obsolete]
        public void LogLoadAvatarFromDialog(string avatarUrl, bool eyeAnimation, bool voiceHandler)
        {
            if (!isEnabled) return;
            amplitudeEventLogger.LogEvent(Constants.EventName.LOAD_AVATAR_FROM_DIALOG, new Dictionary<string, object>()
            {
                { Constants.Properties.AVATAR_URL, avatarUrl },
                { Constants.Properties.EYE_ANIMATION, eyeAnimation },
                { Constants.Properties.VOICE_HANDLER, voiceHandler }
            });
        }

        [Obsolete]
        public void LogUpdatePartnerURL(string previousSubdomain, string newSubdomain)
        {
            if (!isEnabled) return;
            amplitudeEventLogger.LogEvent(Constants.EventName.UPDATED_PARTNER_URL, new Dictionary<string, object>()
            {
                { Constants.Properties.PREVIOUS_SUBDOMAIN, previousSubdomain },
                { Constants.Properties.NEW_SUBDOMAIN, newSubdomain }
            }, new Dictionary<string, object>()
            {
                { Constants.Properties.SUBDOMAIN, newSubdomain }
            });
        }

        [Obsolete]
        public void LogOpenDialog(string dialog)
        {
            if (!isEnabled) return;
            amplitudeEventLogger.LogEvent(Constants.EventName.OPEN_DIALOG, new Dictionary<string, object>()
            {
                { Constants.Properties.DIALOG, dialog }
            });
        }

        [Obsolete]
        public void LogBuildApplication(string target, string appName, bool productionBuild)
        {
            if (!isEnabled) return;
            amplitudeEventLogger.LogEvent(Constants.EventName.BUILD_APPLICATION, new Dictionary<string, object>()
            {
                { Constants.Properties.TARGET, target },
                { Constants.Properties.APP_NAME, appName },
                { Constants.Properties.PRODUCTION_BUILD, productionBuild },
                { Constants.Properties.APP_IDENTIFIER, Application.identifier }
            });
        }

        private void GenerateSessionId()
        {
            amplitudeEventLogger.SetSessionId(DateTimeOffset.Now.ToUnixTimeMilliseconds());
        }

        [Obsolete]
        private void ToggleAnalytics(bool allow)
        {
            if (!isEnabled) return;
            var appData = ApplicationData.GetData();
            amplitudeEventLogger.LogEvent(Constants.EventName.ALLOW_ANALYTICS, new Dictionary<string, object>()
            {
                { Constants.Properties.ALLOW, allow }
            }, new Dictionary<string, object>
            {
                { Constants.Properties.ENGINE_VERSION, appData.UnityVersion },
                { Constants.Properties.RENDER_PIPELINE, appData.RenderPipeline },
                { Constants.Properties.SUBDOMAIN, appData.PartnerName },
                { Constants.Properties.APP_NAME, PlayerSettings.productName },
                { Constants.Properties.SDK_TARGET, "Unity" },
                { Constants.Properties.APP_IDENTIFIER, Application.identifier },
                { Constants.Properties.ALLOW_ANALYTICS, allow }
            });
        }
    }
}
