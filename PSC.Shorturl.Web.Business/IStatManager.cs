using System;
using System.Collections.Generic;
using PSC.Shorturl.Web.Utility.Models;

namespace PSC.Shorturl.Web.Business
{
    public interface IStatManager
    {
        string GenerateRandomColor();
        int GetSegmentId(string segment);
        string GraphColor(int index);
        List<TrafficData> StatSegmentForBrowser(string segment, DateTime dtStart, DateTime dtEnd);
        List<TrafficData> StatSegmentForDevice(string segment, DateTime dtStart, DateTime dtEnd);
        List<object> StatSegmentForGraph(string segment, DateTime dtStart, DateTime dtEnd);
        List<TrafficData> StatSegmentForPlatform(string segment, DateTime dtStart, DateTime dtEnd);
    }
}