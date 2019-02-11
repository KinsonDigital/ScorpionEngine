namespace KDParticleEngine
{
    public class ParticleColor
    {
        public byte Alpha { get; set; }

        public byte Red { get; set; }

        public byte Green { get; set; }

        public byte Blue { get; set; }


        public override bool Equals(object obj)
        {
            var comparee = obj as ParticleColor;

            if (comparee == null)
                return false;

            return Alpha == comparee.Alpha &&
                   Red == comparee.Red &&
                   Green == comparee.Green &&
                   Blue == comparee.Blue;
        }
    }
}
