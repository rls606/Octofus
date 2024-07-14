using Octofus.Options.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Octofus.Views.Visualizer
{
    public class VisualizerModel
    {
        public VisualizerModel()
        {
            characters = new List<Character>();    
        }

        public List<Character> characters;
    }
}
