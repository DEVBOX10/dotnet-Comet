﻿using System;
using System.Collections.Generic;

using System.Text;
using Microsoft.Maui.Graphics;


/*
 
import SwiftUI

struct ContentView: View {
    var body: some View {
        Text("Turtle Rock")
            .font(.title)
            .color(.green)
    }
}

 */

namespace Comet.Samples.Comparisons
{
	public class Section2 : View
	{
		[Body]
		View body() =>
				 new Text("Turtle Rock")
					 .Color(Colors.Green);
	}

}
