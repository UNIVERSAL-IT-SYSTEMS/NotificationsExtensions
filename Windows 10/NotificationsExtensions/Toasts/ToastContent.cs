// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

using System;
using System.Runtime.CompilerServices;
using System.Text;
#if WINRT
using Windows.Data.Xml.Dom;
#endif

[assembly: InternalsVisibleTo("NotificationsExtensions.Win10.Test.Portable")]

namespace NotificationsExtensions.Toasts
{

    /// <summary>
    /// Base toast element, which contains at least a visual element.
    /// </summary>
    public sealed class ToastContent : INotificationContent
    {
        /// <summary>
        /// The visual element is required.
        /// </summary>
        public ToastVisual Visual { get; set; }

        /// <summary>
        /// Specify custom audio options.
        /// </summary>
        public ToastAudio Audio { get; set; }

        /// <summary>
        /// Optionally create custom actions with buttons and inputs (using <see cref="ToastActionsCustom"/>) or optionally use the system-default snooze/dismiss controls (with <see cref="ToastActionsSnoozeAndDismiss"/>).
        /// </summary>
        public IToastActions Actions { get; set; }

        /// <summary>
        /// Specify the scenario, to make the toast behave like an alarm, reminder, or more.
        /// </summary>
        public ToastScenario Scenario { get; set; }

        /// <summary>
        /// The amount of time the toast should display. You typically should use the Scenario attribute instead, which impacts how long a toast stays on screen.
        /// </summary>
        public ToastDuration Duration { get; set; }

        /// <summary>
        /// A string that is passed to the application when it is activated by the toast. The format and contents of this string are defined by the app for its own use. When the user taps or clicks the toast to launch its associated app, the launch string provides the context to the app that allows it to show the user a view relevant to the toast content, rather than launching in its default way.
        /// </summary>
        public string Launch { get; set; }

        /// <summary>
        /// Specifies what activation type will be used when the user clicks the body of this toast.
        /// </summary>
        public ToastActivationType ActivationType { get; set; }

        public string GetContent()
        {
            return ConvertToElement().GetContent();
        }

#if WINRT

        public XmlDocument GetXml()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(GetContent());

            return doc;
        }

#endif

        internal Element_Toast ConvertToElement()
        {
            var toast = new Element_Toast()
            {
                ActivationType = ActivationType,
                Duration = Duration,
                Launch = Launch,
                Scenario = Scenario
            };

            if (Visual != null)
                toast.Visual = Visual.ConvertToElement();

            if (Audio != null)
                toast.Audio = Audio.ConvertToElement();

            if (Actions != null)
                toast.Actions = ConvertToActionsElement(Actions);
            

            return toast;
        }

        private static Element_ToastActions ConvertToActionsElement(IToastActions actions)
        {
            Element_ToastActions converted = ConversionHelper.ConvertToElement(actions) as Element_ToastActions;

            if (converted == null)
                throw new NotImplementedException("Toast actions must support converting to Element_ToastActions");

            return converted;
        }
    }
}