using System;
using System.Collections.Generic;

namespace ImagenarioCS
{
    public class SCX
    {

        static int PLAYER_COUNT = 16;
        static int MESSAGE_COUNT = 6;
        static int CINEMATIC_COUNT = 4;
        static int BEHAVIOR_COUNT = 3;
        static int VICTORY_CONDITION_COUNT = 10;
        static int DISABLED_TECH_COUNT = 30;
        static int DISABLED_UNIT_COUNT = 30;
        static int DISABLED_BUILDING_COUNT = 20;
        static int RESOURCE_COUNT = 7;
        static int NAME_LENGTH = 256;

        public String version = String.Empty;
        public int briefing_length;
        public String briefing = String.Empty;
        public int next_unit_id;
        public float version2;
        public int[] player_count = new int[3];
        public Player[] players = new Player[PLAYER_COUNT];
        public Player gaia = new Player();

        int message_option_0;
        char message_option_1;
        float message_option_2;

        String filename = String.Empty;

        int[] messages_st = new int[MESSAGE_COUNT];
        String[] messages = new String[MESSAGE_COUNT];

        String[] cinematics = new String[CINEMATIC_COUNT];

        int[] victories = new int[VICTORY_CONDITION_COUNT];

        int[] disability_options = new int[3];

        SCX.Bitmap bitmap = new SCX.Bitmap();

        Terrain terrain = new Terrain();

        List<Unit> units = new List<Unit>();

        public SCX()
        {
            for (int i = 0; i < PLAYER_COUNT; ++i)
            {
                this.players[i] = new Player();
            }
        }

        public int addUnit(float x, float y, float z, short constant, short player)
        {
            Random rnd = new Random();
            Unit u = new Unit
            {
                x = x,
                y = y,
                z = z,
                constant = constant,
                player = player,
                rotation = rnd.Next(0, 8),
                id = this.next_unit_id
            };
            ++this.next_unit_id;
            units.Add(u);
            if (player == 0)
                ++this.gaia.unit_count;
            else
                ++this.players[player - 1].unit_count;

            return u.id;
        }

        public int addUnit(int x, int y, int z, short constant, short player)
        {
            return addUnit(x + .5f, y + .5f, z, constant, player);
        }

        public void removeAllUnits()
        {
            units.Clear();
            gaia.unit_count = 0;
            for (int i = 0; i < PLAYER_COUNT; ++i)
            {
                if (players[i] != null)
                    players[i].unit_count = 0;
            }
            this.next_unit_id = 0;
        }


        public class Player
        {
            public String name = string.Empty;
            public string ai = string.Empty;
            public String[] aic = new String[BEHAVIOR_COUNT];
            public int name_st;
            public char aitype;
            public int @bool;
            public int machine;
            public int profile;
            public int unknown;
            public float[] resources = new float[RESOURCE_COUNT];
            public int[] v_diplomacies = new int[PLAYER_COUNT];
            public int alliedvictory, startage;
            public int[] disabled_techs = new int[DISABLED_TECH_COUNT];
            public int[] disabled_units = new int[DISABLED_UNIT_COUNT];
            public int[] disabled_buildings = new int[DISABLED_BUILDING_COUNT];
            public int unit_count = 0;
            public String subtitle = String.Empty;
            public float[] view = new float[2];
            public short[] view2 = new short[2];
            public char allied;
            public int colorid;
            public byte[] special = new byte[0], special2 = new byte[0];
            public byte[] stance1 = new byte[0], stance2 = new byte[0];
        }

        public class Bitmap
        {
            int @bool;
            int width;
            int height;
            short def;
            byte[] bitmap;
        }

    }
}