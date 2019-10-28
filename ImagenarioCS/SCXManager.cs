using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ImagenarioCS
{
    public class SCXManager
    {
        public static SCX load(String scfile)
        {
            SCX scx = new SCX();
            FileInfo scxfile = new FileInfo(scfile);
            int flen = (int) scxfile.Length;

            try
            {
                using (var fileStream = new FileStream(scfile, FileMode.Open))
                {
                    using (var fis = new BinaryReader(fileStream))
                    {

                        //FileInputStream fis = new FileStream();
                        //fis = new FileInputStream(scxfile);

                        byte[] byte16 = new byte[16];
                        byte[] byte4 = new byte[4];
                        byte[] byte2 = new byte[2];
                        byte[] byte1 = new byte[1];

                        // general / version / 1
                        byte4 = fis.ReadBytes(4);
                        Console.WriteLine(ByteArrayToString(byte4));
                        scx.version = byte4.ToString();
                        //System.out.println(scx.version);

                //general / header
                        byte4 = fis.ReadBytes(4);
                        Console.WriteLine(ByteArrayToString(byte4));

                        //general / unknown / 1
                        byte4 = fis.ReadBytes(4);
                        Console.WriteLine(ByteArrayToString(byte4));

                        //general / timestamp
                        byte4 = fis.ReadBytes(4);
                        Console.WriteLine(ByteArrayToString(byte4));
                        //System.currentTimeMillis();

                        //message / briefing
                        byte4 = fis.ReadBytes(4);
                        Console.WriteLine(ByteArrayToString(byte4));

                        int briefing_length = Convert(byte4);
                        //System.out.println(briefing_length);
                        byte[] briefing = new byte[briefing_length];
                        scx.briefing_length = briefing_length;

                        briefing = fis.ReadBytes(briefing_length);
                        scx.briefing = briefing.ToString();
                        //System.out.println(scx.briefing);

                        //general / unknown / 2
                        byte4 = fis.ReadBytes(4);

                        //players / count / 1
                        byte4 = fis.ReadBytes(4);
                        scx.player_count[0] = Convert(byte4);
                        //System.out.println("Player Count:"+scx.player_count[0]);


                        /* ************** BODY PART *************** */
                        //Inflater inf = getBody(fis, flen);

                    }
                }

                //objects / increment
                /*inf.inflate(byte4);
                scx.next_unit_id = Convert(byte4);
                //System.out.println("Next Object ID:"+objects);

                //version / 2
                scx.version2 = $F(inf, byte4);
                //System.out.println(new String(byte4));

                //players / name
                byte[] pname = new byte[SCX.NAME_LENGTH];
                for (int i = 0; i < SCX.PLAYER_COUNT; ++i)
                {
                    inf.inflate(pname);
                    scx.players[i].name = new String(pname).trim();
                    //System.out.println("Player #"+(i+1)+":"+scx.players[i].name);
                }

                //players / string
                for (int i = 0; i < SCX.PLAYER_COUNT; ++i)
                {
                    scx.players[i].name_st = $(inf, byte4);
                }

                //players / config
                for (int i = 0; i < SCX.PLAYER_COUNT; ++i)
                {
                    inf.inflate(byte4);
                    scx.players[i].bool = Convert(byte4);
                    inf.inflate(byte4);
                    scx.players[i].machine = Convert(byte4);
                    inf.inflate(byte4);
                    scx.players[i].profile = Convert(byte4);
                    inf.inflate(byte4);
                    scx.players[i].unknown = Convert(byte4);
//				//System.out.printf("Player #%d: %d %d %d %d\n", i, 
//						scx.players[i].bool, scx.players[i].machine, scx.players[i].profile, scx.players[i].unknown);
                }

                //message / unknowns
                inf.inflate(byte4);
                scx.message_option_0 = Convert(byte4);
                inf.inflate(byte1);
                scx.message_option_1 = (char) byte1[0];
                inf.inflate(byte4);
                scx.message_option_2 = ByteConverter.getFloat(byte4, 0);

                //message / filename
                inf.inflate(byte2);
                short filename_length = ByteConverter.getShort(byte2, 0);
                byte[] filename = new byte[filename_length];
                inf.inflate(filename);
                scx.filename = new String(filename);
                //System.out.println(scx.filename);

                //# message / strings
                //# 0x01 = objective
                //# 0x02 = hints
                //# 0x03 = victory
                //# 0x04 = failure
                //# 0x05 = history
                //# 0x06 = scouts
                for (int i = 0; i < SCX.MESSAGE_COUNT; ++i)
                {
                    scx.messages_st[i] = $(inf, byte4);
                    //System.out.println(scx.messages_st[i]);
                }

                // message / scripts
                readStrings(inf, SCX.MESSAGE_COUNT, scx.messages);

//message / cinematics
                readStrings(inf, SCX.CINEMATIC_COUNT, scx.cinematics);


                //message / bitmap
                {
                    scx.bitmap.bool = $(inf, byte4);
                    scx.bitmap.width = $(inf, byte4);
                    scx.bitmap.height = $(inf, byte4);
                    inf.inflate(byte2);
                    scx.bitmap.def = ByteConverter.getShort(byte2, 0);
                    if (scx.bitmap.bool > 0){
                        byte[] bitmap = new byte[40 + 1024 + scx.bitmap.width * scx.bitmap.height];
                        inf.inflate(bitmap);
                        scx.bitmap.bitmap = bitmap;
                        //System.out.println("Bitmap Get");
                    }else{
                        scx.bitmap.bitmap = new byte[0];
                        //System.out.println("No Bitmap");
                    }
                }

                //behavior / names
                inf.inflate(new byte[SCX.PLAYER_COUNT * (SCX.BEHAVIOR_COUNT - 1) * 2]); // SKIP ALL AOE1 PROPS
                //for (int i=0; i<3; ++i){
                for (int j = 0; j < SCX.PLAYER_COUNT; ++j)
                {
                    inf.inflate(byte2);
                    int length = ByteConverter.getShort(byte2, 0);
                    if (length > 0)
                    {
                        byte[] message = new byte[length];
                        inf.inflate(message);
                        scx.players[j].ai = new String(message);
                    }
                }
                //}

                //behavior / size & data
                for (int i = 0; i < SCX.PLAYER_COUNT; ++i)
                {
                    int[] lengths = new int[SCX.BEHAVIOR_COUNT];
                    for (int j = 0; j < SCX.BEHAVIOR_COUNT; ++j)
                    {
                        lengths[j] = $(inf, byte4);
                    }

                    for (int j = 0; j < SCX.BEHAVIOR_COUNT; ++j)
                    {
                        byte[] message = new byte[lengths[j]];
                        inf.inflate(message);
                        scx.players[i].aic[j] = new String(message);
                    }
                }

                //behavior / type
                inf.inflate(byte16);
                for (int i = 0; i < SCX.PLAYER_COUNT; ++i)
                {
                    scx.players[i].aitype = (char) byte16[i];
                }

                //general / separator / 1
                inf.inflate(byte4);

                //player / config (2)
                inf.inflate(new byte[24 * SCX.PLAYER_COUNT]);

                //general / separator / 2
                inf.inflate(byte4);

                //victory / globals
                //# 0x01 = conquest
                //# 0x02 = ruins
                //# 0x03 = artifacts
                //# 0x04 = discoveries
                //# 0x05 = explored
                //# 0x06 = gold count
                //# 0x07 = required
                //# 0x08 = condition
                //# 0x09 = score
                //# 0x0A = time limit
                for (int i = 0; i < SCX.VICTORY_CONDITION_COUNT; ++i)
                {
                    scx.victories[i] = $(inf, byte4);
                }

                //victory / diplomacy / player / stance
                for (int i = 0; i < SCX.PLAYER_COUNT; ++i)
                {
                    for (int j = 0; j < SCX.PLAYER_COUNT; ++j)
                    {
                        scx.players[i].v_diplomacies[j] = $(inf, byte4);
                    }
                }

                //victory / individual-victory (12 triggers per players) 
                //(they are unused in AoK/AoC once the new trigger system was introduced)
                inf.inflate(new byte[SCX.PLAYER_COUNT * 15 * 12 * 4]);

                //general / separator / 3
                inf.inflate(byte4);
                //System.out.println(Convert(byte4));

                //victory / diplomacy / player / allied
                for (int i = 0; i < SCX.PLAYER_COUNT; ++i)
                {
                    scx.players[i].alliedvictory = $(inf, byte4);
                } //System.out.printf("\n");

                //disability / techtree
                int[] disables =
                    {SCX.DISABLED_TECH_COUNT, SCX.DISABLED_UNIT_COUNT, SCX.DISABLED_BUILDING_COUNT};
                for (int i = 0; i < 3; ++i)
                {
                    inf.inflate(new byte[64]);
                    for (int j = 0; j < SCX.PLAYER_COUNT; ++j)
                    {
                        for (int k = 0; k < disables[i]; ++k)
                        {
                            switch (i)
                            {
                                case 0:
                                    scx.players[j].disabled_techs[k] = $(inf, byte4);
                                    break;
                                case 1:
                                    scx.players[j].disabled_units[k] = $(inf, byte4);
                                    break;
                                case 2:
                                    scx.players[j].disabled_buildings[k] = $(inf, byte4);
                                    break;
                            }

                            //System.out.printf("%d ", scx.players[j].disabled_techs[k]);
                        }

                        //System.out.printf("\n");
                    }
                }

                //disability / options
                for (int i = 0; i < 3; ++i)
                {
                    scx.disability_options[i] = $(inf, byte4);
                }

                //disability / starting age
                for (int i = 0; i < SCX.PLAYER_COUNT; ++i)
                {
                    scx.players[i].startage = $(inf, byte4);
                }

                //general / separator / 4
                inf.inflate(byte4);
                //System.out.println(Convert(byte4));

                //terrain / view
                inf.inflate(byte4);
                inf.inflate(byte4);

                //terrain / type
                inf.inflate(byte4);

                //terrain size
                scx.terrain.sizex = $(inf, byte4);
                scx.terrain.sizey = $(inf, byte4);

                //System.out.printf("%d ", scx.terrain.sizex);

                //terrain / data @TERRAIN
                byte[] byte3 = new byte[3];
                scx.terrain.initializeTiles();
                for (int i = 0; i < scx.terrain.sizey; ++i)
                {
                    for (int j = 0; j < scx.terrain.sizex; ++j)
                    {
                        inf.inflate(byte3);
                        scx.terrain.tiles[j][i] = (char) byte3[0];
                        scx.terrain.hills[j][i] = (char) byte3[1];
                    }
                }

                //players / count / 2
                // GAIA included
                int playercount = $(inf, byte4);
                scx.player_count[1] = playercount;
                //System.out.println(scx.player_count[0]);

                //player / sources & config
                for (int i = 0; i < playercount - 1; ++i)
                {
                    for (int j = 0; j < SCX.RESOURCE_COUNT; ++j)
                    {
                        scx.players[i].resources[j] = $F(inf, byte4);
                        //System.out.printf("%5d ", (int)scx.players[i].resources[j]);
                    }

                    //System.out.printf("\n");
                }

                //objects / players
                for (int i = 0; i < playercount; ++i)
                {
                    int count = $(inf, byte4);
                    //System.out.println("Player #"+i+" Units:"+count);
                    if (i == 0)
                        scx.gaia.unit_count = count;
                    else
                        scx.players[i - 1].unit_count = count;
                    for (int j = 0; j < count; ++j)
                    {
                        Unit u = new Unit();
                        u.x = $F(inf, byte4);
                        u.y = $F(inf, byte4);
                        u.z = $F(inf, byte4);
                        u.id = $(inf, byte4);
                        inf.inflate(byte2);
                        u.constant = ByteConverter.getShort(byte2, 0);
                        inf.inflate(byte1);
                        u.progress = (char) byte1[0];
                        u.rotation = $F(inf, byte4);
                        inf.inflate(byte2);
                        u.frame = ByteConverter.getShort(byte2, 0);
                        u.garrison = $(inf, byte4);
                        u.player = (short) i;
//System.out.printf("%.0f %.0f %.0f %d %d\n", u.x, u.y, u.z, u.id, u.constant);
                        scx.units.add(u);
                    }
                }
                //System.out.println("Total Units:"+scx.units.size());

                //players / count / 3 : Should be 9
                scx.player_count[2] = $(inf, byte4);
                //System.out.println("Players Count 3:"+scx.player_count[2]);

                //
                for (int i = 1; i < playercount; ++i)
                {
                    SCX.Player player = scx.players[i - 1];

//player / script
                    short len = $S(inf, byte2);
//System.out.println(len);
                    byte[] subtitle = new byte[len];
                    inf.inflate(subtitle);
                    player.subtitle = new String(subtitle);
//System.out.println(player.subtitle);

//player / views
                    player.view[0] = $F(inf, byte4);
                    player.view[1] = $F(inf, byte4);
//System.out.println(player.view[0]+", "+player.view[1]);

                    player.view2[0] = $S(inf, byte2);
                    player.view2[1] = $S(inf, byte2);
//System.out.println(player.view2[0]+", "+player.view2[1]);

//player / diplomacy
                    player.allied = $C(inf, byte1);
                    int alliedcount = $S(inf, byte2);
//diplomacy / stance / 1
                    player.stance1 = new byte[alliedcount];
                    inf.inflate(player.stance1);
                    //System.out.println(new String(stance1));
                    //diplomacy / stance / 2
                    player.stance2 = new byte[alliedcount << 2];
                    inf.inflate(player.stance2);
                    //System.out.println(new String(stance2));

                    //player / color
                    player.colorid = $(inf, byte4);
                    //System.out.println(player.colorid);

                    //player / victory / version
                    float number = $F(inf, byte4);

//player / victory / triggers / count
                    short triggers = $S(inf, byte2);

                    //player / victory / values
                    if ((int) number == 2)
                    {
                        player.special = new byte[8];
                        inf.inflate(player.special);
                    }

                    //player / triggers / trigger (the ancient triggers
                    inf.inflate(new byte[triggers * 11 * 4]);

                    //player / unknown
                    player.special2 = new byte[7];
                    inf.inflate(player.special2);

                        //player / victory / unknown
                        $(inf, byte4);

                }

                inf.end();*/

                return scx;
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.ToString());
            }

            return null;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2} ", b);
            return hex.ToString();
        }


        /*static Inflater getBody(BinaryReader fis, int length)
        {
            try
            {

                byte[] filecontent = new byte[length];

                filecontent = fis.ReadBytes(length);

                Inflater inf = new Inflater(true);

                inf.setInput(filecontent, 0, length);

                return inf;

            }
            catch (IOException e1)
            {
                e1.printStackTrace();
            }
            return null;
        }*/

        public static int Convert(byte[] bytes){
            return ByteConverter.byteArray2int(bytes, true);
        }
}


}