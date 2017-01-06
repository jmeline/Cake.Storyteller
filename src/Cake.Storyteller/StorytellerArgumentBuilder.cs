﻿using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Storyteller
{
    public class StorytellerArgumentBuilder
    {
        private readonly List<string> _stArguments;
        public StorytellerArgumentBuilder()
        {
            _stArguments = new List<string>
            {
                 "results-path" ,
                 "workspace" ,
                 "exclude-tags" ,
                 "open" ,
                 "csv" ,
                 "json" ,
                 "dump" ,
                 "build" ,
                 "profile" ,
                 "timeout" ,
                 "lifecycle" ,
                 "teamcity" ,
                 "config" ,
                 "retries" 
            };
        }

        public ProcessArgumentBuilder BuildArguments(ICakeArguments arguments, StorytellerSettings settings = null)
        {
            var dict = new Dictionary<string, string>();

            if (settings != null)
            {
                //settings
                var props = typeof(StorytellerSettings).GetProperties();
                foreach (var prop in props)
                {
                    var attrs = prop.GetCustomAttributes(false);
                    foreach (var attr in attrs)
                    {
                        var stFlagAttr = attr as StorytellerFlag;
                        if (stFlagAttr != null)
                        {
                            var value = prop.GetValue(settings, null);
                            if (value != null)
                            {
                                dict.Add(stFlagAttr.Flag, value.ToString());
                            }
                        }
                    }
                }
            }

            //arguments
            if (arguments != null)
            {
                _stArguments.ForEach(arg =>
                {
                    if (arguments.HasArgument(arg))
                    {
                        dict[arg] = arguments.GetArgument(arg);
                    }
                });
            }

            var builder = new ProcessArgumentBuilder();
            foreach (var pair in dict)
            {
                builder.Append("--" + pair.Key + " " + pair.Value + " ");
            }
            return builder;
        }

        public ProcessArgumentBuilder BuildArguments(StorytellerSettings settings)
        {
            return BuildArguments(null, settings);
        }
    }
}