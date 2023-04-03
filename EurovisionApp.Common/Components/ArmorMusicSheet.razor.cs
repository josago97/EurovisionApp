using System.Text;
using Microsoft.AspNetCore.Components;

namespace EurovisionApp.Common.Components
{
    public partial class ArmorMusicSheet
    {
        public enum Notes { CFlat, C, CSharp, DFlat, D, DSharp, EFlat, E, ESharp, FFlat, F, FSharp, GFlat, G, GSharp, AFlat, A, ASharp, BFlat, B, BSharp };
        public enum Scales { Major, Minor };

        private const int TEMPO_POSITION_Y = 10;
        private const int STAFF_POSITION_Y = TEMPO_POSITION_Y + 15;
        private const int STAFF_WIDTH = 90;
        private const int STAFF_SEPARATION = 6;
        private const int ALTERATION_SEPARATION = 8;
        private static readonly Notes[] SHARPS_ORDER = { Notes.C, Notes.G, Notes.D, Notes.A, Notes.E, Notes.B, Notes.FSharp, Notes.CSharp };
        private static readonly double[] SHARP_POSITIONS = { 0, 1.5, -0.5, 1, 2.5, 0.5, 2 }; // F C G D A E B
        private static readonly Notes[] FLATS_ORDER = { Notes.C, Notes.F, Notes.BFlat, Notes.EFlat, Notes.AFlat, Notes.DFlat, Notes.GFlat, Notes.CFlat };
        private static readonly double[] FLAT_POSITIONS = { 2, 0.5, 2.5, 1, 3, 1.5, 3.5 }; // B E A D G C F
        private static readonly Dictionary<Notes, Notes> FIFTH_CIRCLE = new Dictionary<Notes, Notes>() // Minor -> Major
        {
            // 0
            { Notes.A, Notes.C },
            // 1
            { Notes.E, Notes.G },
            { Notes.D, Notes.A },
            // 2
            { Notes.G, Notes.BFlat },
            { Notes.B, Notes.D },
            // 3
            { Notes.C, Notes.EFlat },
            { Notes.FSharp, Notes.A },
            // 4
            { Notes.F, Notes.AFlat },
            { Notes.CSharp, Notes.E },
            // 5
            { Notes.BFlat, Notes.DFlat },
            { Notes.GSharp, Notes.B },
            // 6
            { Notes.DSharp, Notes.FSharp },
            { Notes.EFlat, Notes.GFlat },
            // 7
            { Notes.AFlat, Notes.CFlat },
            { Notes.ASharp, Notes.CSharp },
        };

        [Parameter]
        public Notes Tone { get; set; }
        [Parameter]
        public Scales Scale { get; set; }
        [Parameter]
        public int? Tempo { get; set; }

        private string Html { get; set; }

        protected override void OnParametersSet()
        {
            StringBuilder htmlBuilder = new StringBuilder();
            if (Tempo.HasValue) DrawTempo(htmlBuilder, Tempo.Value);
            DrawStaff(htmlBuilder);
            DrawArmor(htmlBuilder);

            Html = htmlBuilder.ToString();
        }

        private void DrawTempo(StringBuilder builder, int tempo)
        {
            builder.AppendLine(DrawText(0, TEMPO_POSITION_Y, 12, "Polihymnia", "qj"));
            builder.AppendLine(DrawText(11, TEMPO_POSITION_Y, 8, "Open Sans", $"= {tempo}"));
        }

        private void DrawStaff(StringBuilder stringBuilder)
        {
            int y = STAFF_POSITION_Y;

            for (int i = 0; i < 5; i++)
            {
                stringBuilder.AppendLine(DrawLine(0, y, STAFF_WIDTH, y));
                y += STAFF_SEPARATION;
            }

            stringBuilder.AppendLine(DrawText(3, STAFF_POSITION_Y + 16, 22, "Polihymnia", "Gj"));
        }

        private void DrawArmor(StringBuilder builder)
        {
            Notes tone = Tone;

            // Find relative major
            if (Scale == Scales.Minor && !FIFTH_CIRCLE.TryGetValue(tone, out tone))
            {
                throw new InvalidDataException("Not exist scale");
            }

            string alteration;
            double[] alterationPositions;
            int alterationCount = Array.IndexOf(SHARPS_ORDER, tone);

            if (alterationCount >= 0) // Sharp
            {
                alteration = "Xj";
                alterationPositions = SHARP_POSITIONS;
            }
            else
            {
                alterationCount = Array.IndexOf(FLATS_ORDER, tone);

                if (alterationCount >= 0) // Flat
                {
                    alteration = "bj";
                    alterationPositions = FLAT_POSITIONS;
                }
                else
                    throw new InvalidDataException("Not exist scale");
            }

            for (int i = 0; i < alterationCount; i++)
            {
                int y = (int)(STAFF_POSITION_Y + alterationPositions[i] * STAFF_SEPARATION);

                builder.AppendLine(DrawText(25 + ALTERATION_SEPARATION * i, y, 22, "Polihymnia", alteration));
            }
        }

        private string DrawLine(double x1, double y1, double x2, double y2)
        {
            return $"<line x1=\"{x1}\" y1=\"{y1}\" x2=\"{x2}\" y2=\"{y2}\" style=\"fill:none;stroke:rgb(0,0,0);stroke-width:0.5\"></line>";
        }

        private string DrawText(double x, double y, double fontSize, string fontFamily, string innerHtml)
        {
            return $"<text x=\"{x}\" y=\"{y}\" style=\"fill:rgb(0,0,0); font-size:{fontSize}pt; font-family: {fontFamily};\">{innerHtml}</text>";
        }
    }
}