﻿using System.Collections.Generic;

namespace PSC.Chartjs.Mvc.ComplexChart
{
    /// <summary>
    /// The complex dataset.
    /// </summary>
    public class ComplexDataset
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexDataset"/> class.
        /// </summary>
        public ComplexDataset()
        {
            this.Data = new List<double>();
        }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        public string Label
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the fill color.
        /// </summary>
        public string FillColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the stroke color.
        /// </summary>
        public string StrokeColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the point color.
        /// </summary>
        public string PointColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the point stroke color.
        /// </summary>
        public string PointStrokeColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the point highlight fill.
        /// </summary>
        public string PointHighlightFill
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the point highlight stroke.
        /// </summary>
        public string PointHighlightStroke
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public List<double> Data
        {
            get; 
            set;
        }
    }
}