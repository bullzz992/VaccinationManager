using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VaccinationManager.Models;

namespace VaccinationManager.ViewModels
{
    public class MeasurementViewModel
    {
        public ChildMeasurement CaptureMeasurement { get; set; }
        public List<ChildMeasurement> Measurements { get; set; }

        public List<string> HeadData { get; set; }
        public List<string> WeightData { get; set; }
        public List<string> HeightData { get; set; }
        public List<string> LabelData { get; set; }

        public MeasurementViewModel() {
            CaptureMeasurement = new ChildMeasurement();
        }

        public MeasurementViewModel(List<ChildMeasurement> measures) : this()
        {
            Measurements = measures;

            HeadData = measures.Select(m => m.HeadCircumference.ToString()).ToList();
            WeightData = measures.Select(m => m.Weight.ToString()).ToList();
            HeightData = measures.Select(m => m.Height.ToString()).ToList();
            LabelData = measures.Select(m => m.Created.ToShortDateString()).ToList();

            //HeadData = JsonConvert.SerializeObject(headData, Formatting.None);//.Replace("\\", "");
            //WeightData = JsonConvert.SerializeObject(weighthData, Formatting.None);//.Replace("\\", "");
            //HeightData = JsonConvert.SerializeObject(heigthData, Formatting.None);//.Replace("\\", "");
            //LabelData = JsonConvert.SerializeObject(measureDates, Formatting.None);//.Replace("\\", "");
        }
    }
}