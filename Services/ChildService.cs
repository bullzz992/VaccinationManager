using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using VaccinationManager.Models;

namespace VaccinationManager.Services
{
    public class ChildService
    {
        public string IdNumber { get; set; }
        public Dictionary<string, Child> Children { get; set; }
        public Dictionary<string, string> SessionChild { get; set; }
        public Dictionary<string, string> ChildMeasurementCharts { get; set;}
        public ChildService()
        {
            Children = new Dictionary<string, Child>();
            SessionChild = new Dictionary<string, string>();
            ChildMeasurementCharts = new Dictionary<string, string>();
        }

        public Child GetChild(string idNumber)
        {
            return Children[idNumber];
        }

        public Child GetChildForSession(string sessionId)
        {
            try
            {
                string idNumber = SessionChild[sessionId];
                return GetChild(idNumber);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Failed to get child for session :{0}", ex.Message);
                return null;
            }
        }

        public void AddChild(Child child)
        {
            Children[child.IdNumber] = child;
        }

        private void SetChildIdForSession(string sessionId, string idNumber)
        {
            SessionChild[sessionId] = idNumber;
        }

        public void SetChildForSession(string sessionId, Child child)
        {
            SetChildIdForSession(sessionId, child.IdNumber);
            AddChild(child);
        }

        public void SetMeasurementChart(string sessionId, string imageData)
        {
            try
            {
                string idNumber = SessionChild[sessionId];
                ChildMeasurementCharts[idNumber] = imageData;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Failed to add chart on child set for current session : {0}", ex.Message);
            }
        }

        public string GetMeasurementChart(string idNumber)
        {
            return ChildMeasurementCharts[idNumber];
        }

        public string GetMeasurementChartForSession(string sessionId)
        {
            try
            {
                string idNumber = SessionChild[sessionId];
                return GetMeasurementChart(idNumber);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get child chart for session :{0}", ex.Message);
                return null;
            }
        }
    }
}