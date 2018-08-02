using System.Collections.Generic;

namespace Model
{
    public enum Direction : byte
    {
        PositiveX = 0, Straight = 0,
        PositiveZ = 1, Right = 1,
        NegativeX = 2, Back = 2,
        NegativeZ = 3, Left = 3,
        Max,
        Invalid = byte.MaxValue
    }

    public static class DirectionExtensions
    {
        public static Direction Opposite(this Direction x)
        {
            if (x >= Direction.Max)
                return x;
            return (Direction) ((int) (x + 2) % 4);
        }

        public static Direction Relative(this Direction x, Direction relative)
        {
            if (x > Direction.Max)
                return x;
            return (Direction) (((int)x + (int)relative) % 4);
        }
    }

    public struct Coord
    {
        public int x, y, z;

        public Coord(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        public bool Equals(Coord other)
        {
            return x == other.x && y == other.y && z == other.z;
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x;
                hashCode = (hashCode * 397) ^ y;
                hashCode = (hashCode * 397) ^ z;
                return hashCode;
            }
        }
        
        public override bool Equals(object obj)
        {
            return (obj is Coord) && ((Coord) obj) == this;
        }
        
        public static bool operator == (Coord a, Coord b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(Coord a, Coord b)
        {
            return !(a == b);
        }
        
        public class ComparerClass : IEqualityComparer<Coord>
        {
            public bool Equals(Coord x, Coord y) => x == y;
            public int GetHashCode(Coord obj) => obj.GetHashCode();
        }

        public static Coord operator +(Coord c, Direction dir)
        {
            switch (dir)
            {
                case Direction.PositiveX: c.x += 1; break;
                case Direction.PositiveZ: c.z += 1; break;
                case Direction.NegativeX: c.x -= 1; break;
                case Direction.NegativeZ: c.z -= 1; break;
            }

            return c;
        }
        
        public static Coord operator -(Coord c, Direction dir)
        {
            switch (dir)
            {
                case Direction.PositiveX: c.x -= 1; break;
                case Direction.PositiveZ: c.z -= 1; break;
                case Direction.NegativeX: c.x += 1; break;
                case Direction.NegativeZ: c.z += 1; break;
            }

            return c;
        }
    }
    
    public struct MachinePos
    {
        public Coord pos;
        public Direction direction;

        public Direction back => direction.Opposite();

        public MachinePos(Coord pos, Direction direction)
        {
            this.pos = pos;
            this.direction = direction;
        }
        
        public bool Equals(MachinePos other)
        {
            return pos.Equals(other.pos) && direction == other.direction;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (pos.GetHashCode() * 397) ^ (int) direction;
            }
        }

        public override bool Equals(object obj)
        {
            return (obj is MachinePos) && ((MachinePos) obj) == this;
        }
        
        public static bool operator == (MachinePos a, MachinePos b)
        {
            return a.pos == b.pos && a.direction == b.direction;
        }

        public static bool operator !=(MachinePos a, MachinePos b)
        {
            return !(a == b);
        }

        public static readonly ComparerClass Comparer = new ComparerClass();
        public class ComparerClass : IEqualityComparer<MachinePos>
        {
            public bool Equals(MachinePos x, MachinePos y) => x == y;
            public int GetHashCode(MachinePos obj) => obj.GetHashCode();
        }

        public MachinePos GetConnected()
        {
            return new MachinePos(pos+direction, direction.Opposite());
        }

        public MachinePos Back()
        {
            return new MachinePos(pos, direction.Opposite());
        }

        public static implicit operator Coord(MachinePos pos) => pos.pos;
    }
}