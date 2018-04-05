using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vault_Prisoner
{
    static class GridTextures
    {
        private static List<Texture2D> m;

        public static List<Texture2D> SetupGridMaps(List<Texture2D> maps, int gridCount, Random rng)
        {
            m = maps;
            List<Texture2D> ordered = new List<Texture2D>();

            //Save a copy of maps
            List<Texture2D> copy = new List<Texture2D>();
            for(int i = 0; i < m.Count; i++)
            {
                copy.Add(m[i]);
            }

            if(copy.Count >= gridCount)
            {
                for (int i = 0; i < gridCount; i++)
                {
                    Texture2D map;
                    if (m.Count == 1)
                    {
                        map = copy[0];
                        ordered.Add(map);
                        copy.Remove(map);
                        break;
                    }

                    //Pick a random grid and add it to ordered list
                    map = copy[rng.Next(0, copy.Count)];
                    ordered.Add(map);

                    //Remove assigned map from the map list
                    copy.Remove(map);
                }
            }

            return ordered;
        }
    }
}
