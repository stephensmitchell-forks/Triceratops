﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Newtonsoft.Json;

namespace Triceratops
{
    public class LineDashedMaterial : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the LineDashedMaterial class.
        /// </summary>
        public LineDashedMaterial()
          : base("LineDashedMaterial", "LineDashMat",
              "Description",
              "Triceratops", "Materials")
        {
        }

        // Place in a partition
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddColourParameter("Color", "C", "The line color", GH_ParamAccess.item);
            pManager.AddNumberParameter("LineWidth", "W", "Line width", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("DashSize", "D", "The length of the dash", GH_ParamAccess.item, 3);
            pManager.AddNumberParameter("GapSize", "G", "The length of the gap", GH_ParamAccess.item, 1);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("JSON", "J", "Material's JSON string", GH_ParamAccess.item);
            pManager.AddGenericParameter("Line Material", "M", "The line material object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Declare variables
            Color color = Color.White;
            double linewidth = 1;
            double dashSize = 1;
            double gapSize = 1;

            // Reference the inputs
            DA.GetData(0, ref color);
            DA.GetData(1, ref linewidth);
            DA.GetData(2, ref dashSize);
            DA.GetData(3, ref gapSize);

            // Build the material object
            dynamic material = new ExpandoObject();
            material.uuid = Guid.NewGuid();
            material.type = "LineDashedMaterial";
            material.color = Convert.ToInt32(color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2"), 16);
            material.linewidth = linewidth;
            material.dashSize = dashSize;
            material.gapSize = gapSize;

            // Wrap the material
            MaterialWrapper wrapper = new MaterialWrapper(material);

            // Serialize
            string JSON = JsonConvert.SerializeObject(material);

            DA.SetData(0, JSON);
            DA.SetData(1, wrapper);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.LineDashedMaterial;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2b431a61-7766-40cf-99f4-7753206d0298"); }
        }
    }
}