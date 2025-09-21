using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abilify.Models;

namespace Abilify.Services
{
    class MicroAppRegistry
    {
        public static List<MicroApp> All => new List<MicroApp>
        {
            new MicroApp
            {
                Name = "Read-It (Text-to-Speech Reader)",
                Icon = "readit_icon.png",
                Route = "readit",
                Tags = new List<string> { "Blind", "Low Vision", "Dyslexia", "Learning Difficulties" }
            },
            new MicroApp
            {
                Name = "Focus Timer",
                Icon = "focustimer_icon.png",
                Route = "focustimer",
                Tags = new List<string> { "Autism", "ADHD", "Anxiety", "Memory Loss" }
            },
            new MicroApp
            {
                Name = "Colour Blind Image Processor",
                Icon = "colourblind_icon.png",
                Route = "colourblind",
                Tags = new List<string> { "Colour Blind" }
            },

            new MicroApp
            {
                Name = "Simplify Text",
                Icon = "simplify_icon.png",
                Route = "simplify",
                Tags = new List<string> { "Autism", "ADHD", "Anxiety", "Dyslexia", "Learning Difficulties", "Speech Impairment" }
            },

            new MicroApp
            {
                Name = "Assistance Tool",
                Icon = "assistance_icon.png",
                Route = "assistance",
                Tags = new List<string> { "Autism", "Anxiety", "Depression", "Wheelchair User", "Mobility Impairment", "Communication Support" }
            },
        };
    }
}
