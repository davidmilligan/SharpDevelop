﻿// Copyright (c) 2014 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.PythonBinding;
using NUnit.Framework;
using PythonBinding.Tests.Utils;

namespace PythonBinding.Tests.Designer
{
	/// <summary>
	/// Tests that the controls are initialized in the order they were put on the form. 
	/// The forms designer has them in reverse order in the Controls collection.
	/// </summary>
	[TestFixture]
	public class GeneratedControlOrderingTestFixture
	{
		string generatedPythonCode;
		
		[TestFixtureSetUp]
		public void SetUpFixture()
		{
			using (DesignSurface designSurface = new DesignSurface(typeof(Form))) {
				IDesignerHost host = (IDesignerHost)designSurface.GetService(typeof(IDesignerHost));
				Form form = (Form)host.RootComponent;			
				form.ClientSize = new Size(284, 264);
				
				PropertyDescriptorCollection descriptors = TypeDescriptor.GetProperties(form);
				PropertyDescriptor namePropertyDescriptor = descriptors.Find("Name", false);
				namePropertyDescriptor.SetValue(form, "MainForm");
				
				Button button = (Button)host.CreateComponent(typeof(Button), "button1");
				button.Location = new Point(0, 0);
				button.Size = new Size(10, 10);
				button.Text = "button1";
				button.TabIndex = 0;
				button.UseCompatibleTextRendering = false;
				form.Controls.Add(button);

				RadioButton radioButton = (RadioButton)host.CreateComponent(typeof(RadioButton), "radioButton1");
				radioButton.Location = new Point(20, 0);
				radioButton.Size = new Size(10, 10);
				radioButton.Text = "radioButton1";
				radioButton.TabIndex = 1;
				radioButton.UseCompatibleTextRendering = false;
				form.Controls.Add(radioButton);
				
				DesignerSerializationManager serializationManager = new DesignerSerializationManager(host);
				using (serializationManager.CreateSession()) {
					PythonCodeDomSerializer serializer = new PythonCodeDomSerializer("    ");
					generatedPythonCode = serializer.GenerateInitializeComponentMethodBody(host, serializationManager, String.Empty, 1);
				}
			}
		}
		
		[Test]
		public void GeneratedCode()
		{
			string expectedCode = "    self._button1 = System.Windows.Forms.Button()\r\n" +
								"    self._radioButton1 = System.Windows.Forms.RadioButton()\r\n" +
								"    self.SuspendLayout()\r\n" +
								"    # \r\n" +
								"    # button1\r\n" +
								"    # \r\n" +
								"    self._button1.Location = System.Drawing.Point(0, 0)\r\n" +
								"    self._button1.Name = \"button1\"\r\n" +
								"    self._button1.Size = System.Drawing.Size(10, 10)\r\n" +
								"    self._button1.TabIndex = 0\r\n" +
								"    self._button1.Text = \"button1\"\r\n" +				
								"    # \r\n" +
								"    # radioButton1\r\n" +
								"    # \r\n" +
								"    self._radioButton1.Location = System.Drawing.Point(20, 0)\r\n" +
								"    self._radioButton1.Name = \"radioButton1\"\r\n" +
								"    self._radioButton1.Size = System.Drawing.Size(10, 10)\r\n" +
								"    self._radioButton1.TabIndex = 1\r\n" +
								"    self._radioButton1.Text = \"radioButton1\"\r\n" +				
								"    # \r\n" +
								"    # MainForm\r\n" +
								"    # \r\n" +
								"    self.ClientSize = System.Drawing.Size(284, 264)\r\n" +
								"    self.Controls.Add(self._button1)\r\n" +
								"    self.Controls.Add(self._radioButton1)\r\n" +
								"    self.Name = \"MainForm\"\r\n" +
								"    self.ResumeLayout(False)\r\n";
			
			Assert.AreEqual(expectedCode, generatedPythonCode, generatedPythonCode);
		}
	}
}
