/*
 * ultrasap.cs - create statistics of high tones in SAP files
 *
 * Copyright (C) 2011  Piotr Fusik
 *
 * This file is part of ASAP (Another Slight Atari Player),
 * see http://asap.sourceforge.net
 *
 * ASAP is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License as published
 * by the Free Software Foundation; either version 2 of the License,
 * or (at your option) any later version.
 *
 * ASAP is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with ASAP; if not, write to the Free Software Foundation, Inc.,
 * 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
 */

using System;
using System.IO;

using Sf.Asap;

public class UltraSap
{
	static void Check(ASAP asap, int[] n, int periodCycles, int audc)
	{
		if (periodCycles > 112)
			return;
		if ((audc & 0xf) == 0)
			return;
		audc >>= 4;
		if (audc != 10 && audc != 14)
			return;
		n[periodCycles / 10]++;
	}

	public static int Main(string[] args)
	{
		if (args.Length == 0) {
			Console.WriteLine("Usage: ultrasap path/to/ASMA");
			return 1;
		}
		string asma = args[0];
		ASAP asap = new ASAP();
		byte[] buffer = new byte[885];
		foreach (string file in Files) {
			byte[] module = File.ReadAllBytes(Path.Combine(asma, file));
			asap.Load(file, module, module.Length);
			ASAPInfo info = asap.GetInfo();
			for (int song = 0; song < info.GetSongs(); song++) {
				int duration = info.GetDuration(song);
				asap.PlaySong(song, duration);
				int[] n = new int[12];
				while (asap.Generate(buffer, buffer.Length, ASAPSampleFormat.U8) == buffer.Length) {
					Check(asap, n, asap.Pokeys.BasePokey.PeriodCycles1, asap.Pokeys.BasePokey.Audc1);
					Check(asap, n, asap.Pokeys.BasePokey.PeriodCycles2, asap.Pokeys.BasePokey.Audc2);
					Check(asap, n, asap.Pokeys.BasePokey.PeriodCycles3, asap.Pokeys.BasePokey.Audc3);
					Check(asap, n, asap.Pokeys.BasePokey.PeriodCycles4, asap.Pokeys.BasePokey.Audc4);
				}
				foreach (int i in n) {
					if (i > 0) {
						Console.Write(info.GetSongs() == 1 ? file : file + "#" + (song + 1));
						foreach (int j in n)
							Console.Write("\t{0}", j);
						Console.WriteLine();
						break;
					}
				}
			}
		}
		return 0;
	}

	// files with high frequencies as identified by chksap.pl -s --features
	static readonly string[] Files = {
		"Composers/Anal_lysator/Bhogoosh.sap",
		"Composers/Anal_lysator/Seban_Ty_Prosiaq.sap",
		"Composers/Armijo_Richard_J/Cipher_of_Moving_Compass.sap",
		"Composers/Armijo_Richard_J/Congrabalation.sap",
		"Composers/Armijo_Richard_J/Congrabalation_Mono.sap",
		"Composers/Armijo_Richard_J/Pokey_Axel_Foley.sap",
		"Composers/Armijo_Richard_J/Those_Were_the_Good_Old_Da.sap",
		"Composers/Badkowski_Marek/Deafman.sap",
		"Composers/Badkowski_Marek/Drunklob.sap",
		"Composers/Badkowski_Marek/Full_Shock.sap",
		"Composers/Badkowski_Marek/Mental_Age_End.sap",
		"Composers/Badkowski_Marek/Nemesis.sap",
		"Composers/Balkowski_Krzysztof/Intro_X.sap",
		"Composers/Balkowski_Krzysztof/sEXTOn_Is_Come_in.sap",
		"Composers/Banas_Pawel/Shadow.sap",
		"Composers/Banaszkiewicz_Marcin/Arena_Bzdur_Intro.sap",
		"Composers/Banaszkiewicz_Marcin/Arena_Bzdur_Menu.sap",
		"Composers/Banaszkiewicz_Michal/Hugo_1.sap",
		"Composers/Banaszkiewicz_Michal/Hugo_2.sap",
		"Composers/Banaszkiewicz_Michal/Hugo_6.sap",
		"Composers/Bernasek_Jiri/Easy_Demo.sap",
		"Composers/Bienias_Adam/Bitter_Reality_Flexible_Sq.sap",
		"Composers/Bienias_Adam/Bitter_Reality_Greetings_P.sap",
		"Composers/Bienias_Adam/Dimension_X.sap",
		"Composers/Bienias_Adam/Hitsquad.sap",
		"Composers/Bienias_Adam/Impression.sap",
		"Composers/Bienias_Adam/Just_Fancy.sap",
		"Composers/Bienias_Adam/LFF_Intro.sap",
		"Composers/Bienias_Adam/Little_3.sap",
		"Composers/Bienias_Adam/Ojej.sap",
		"Composers/Billyard_Adam/Henri.sap",
		"Composers/Brooke_Jason/Feud.sap",
		"Composers/Bruenninski_Uwe/Synthy.sap",
		"Composers/Ce_Pumpkin/Animkomials_Meets_Main.sap",
		"Composers/Ce_Pumpkin/Bauvaneck.sap",
		"Composers/Ce_Pumpkin/FrayerSmarker_Absent_Absyn.sap",
		"Composers/Ce_Pumpkin/MyIDE_Demo_Zwei.sap",
		"Composers/Ce_Pumpkin/Priceless.sap",
		"Composers/Ce_Pumpkin/R0l0_8.sap",
		"Composers/Ce_Pumpkin/Revenge_of_Ce_Pumpkin.sap",
		"Composers/Ce_Pumpkin/Rolo_3.sap",
		"Composers/Ce_Pumpkin/Rolo_6.sap",
		"Composers/Ce_Pumpkin/Rulez_2.sap",
		"Composers/Ce_Pumpkin/Songanim.sap",
		"Composers/Ce_Pumpkin/Too_Hard_3.sap",
		"Composers/Ce_Pumpkin/Too_Hard_4_1.sap",
		"Composers/Ce_Pumpkin/Too_Hard_4_2.sap",
		"Composers/Ce_Pumpkin/Too_Hard_4_Pomyje.sap",
		"Composers/Cierkonski_Michal/Ergo_Bibamus_Trackmo.sap",
		"Composers/Cierkonski_Michal/Fuckir.sap",
		"Composers/Cierkonski_Michal/Ja_jo.sap",
		"Composers/Cierkonski_Michal/Kaszana.sap",
		"Composers/Cierkonski_Michal/Kowboj.sap",
		"Composers/Cierkonski_Michal/Mixer_1.sap",
		"Composers/Cierkonski_Michal/Scene_Register_5_0_Intro.sap",
		"Composers/Cierkonski_Michal/Schizophrenics_Vision.sap",
		"Composers/Cierkonski_Michal/The_Dream_3.sap",
		"Composers/Cierkonski_Michal/Yeah_Yeah.sap",
		"Composers/Cierkonski_Michal/Yo.sap",
		"Composers/Cieslewicz_Tomasz/Koziol.sap",
		"Composers/Cieslewicz_Tomasz/Nasze.sap",
		"Composers/Claas_Clever/Time_to_Enjoy_Part_2.sap",
		"Composers/Cornelius_Orall/Ruff_And_Reddy.sap",
		"Composers/Cornelius_Orall/Sidewinder_2.sap",
		"Composers/Cornelius_Orall/Slingshot.sap",
		"Composers/Cornelius_Orall/Speed_Hawk.sap",
		"Composers/Cornelius_Orall/Super_Soccer.sap",
		"Composers/Cornelius_Orall/Tiger_Attack.sap",
		"Composers/Czartynski_Marcin/Demo.sap",
		"Composers/Czartynski_Marcin/Gravity_97_Invitro.sap",
		"Composers/Czartynski_Marcin/Heretic_Crew_Limited_Dem_3.sap",
		"Composers/Czartynski_Marcin/Journey.sap",
		"Composers/Czartynski_Marcin/Journey_Alternative.sap",
		"Composers/Czartynski_Marcin/Juzek.sap",
		"Composers/Czartynski_Marcin/Rush_Hours_97_Invitro.sap",
		"Composers/Czartynski_Marcin/Saper_Konstruktor.sap",
		"Composers/Czartynski_Marcin/Slideshit_Intro.sap",
		"Composers/Czartynski_Marcin/Titus_Slideshow.sap",
		"Composers/Czlowiek_W_Atomizerze/500_Proc_Intro_Mustak.sap",
		"Composers/Czlowiek_W_Atomizerze/Piua_TarCZoWA.sap",
		"Composers/Czlowiek_W_Atomizerze/p0maranCZ0WA_plAma.sap",
		"Composers/Diepenhorst_Marius/I_Will_Survive.sap",
		"Composers/Diepenhorst_Marius/JHV_2k2_Demo.sap",
		"Composers/Dojwa_Jacek/Jumping_Jack_Ingame.sap",
		"Composers/Drozdowski_Marek/Help_Owsiak.sap",
		"Composers/Drozdowski_Marek/Lemmings_Prevision_Intro.sap",
		"Composers/Duesterhoeft_Stephan/ABBUC_8.sap",
		"Composers/Duma_Dariusz/Bug_in_the_Mug.sap",
		"Composers/Duma_Dariusz/Day_Without_Idea.sap",
		"Composers/Duma_Dariusz/Follow_Your_Dream.sap",
		"Composers/Duma_Dariusz/Izolation.sap",
		"Composers/Duma_Dariusz/Lay_Down_and_Enjoy_It.sap",
		"Composers/Duma_Dariusz/Murder_Intro.sap",
		"Composers/Duma_Dariusz/Natural_Excess.sap",
		"Composers/Duma_Dariusz/One_Fast_Day.sap",
		"Composers/Duma_Dariusz/Rotten_Juice_Intro.sap",
		"Composers/Duma_Dariusz/Smooth_Loop.sap",
		"Composers/Duma_Dariusz/Superboy.sap",
		"Composers/Eisenhammer_Zdenek/Deus_Ex_Machina.sap",
		"Composers/Eisenhammer_Zdenek/Diamondz_Ingame.sap",
		"Composers/Eisenhammer_Zdenek/Diamondz_Title.sap",
		"Composers/Eisenhammer_Zdenek/Jozin_z_bazin.sap",
		"Composers/Eisenhammer_Zdenek/Krakout.sap",
		"Composers/Eisenhammer_Zdenek/Mind_Blast_Ingame.sap",
		"Composers/Eisenhammer_Zdenek/Spellbound_Partial_Version.sap",
		"Composers/Esquivel_Sal/ActRaiser_Level_1_1.sap",
		"Composers/Esquivel_Sal/ActRaiser_Level_1_1_Stereo.sap",
		"Composers/Esquivel_Sal/Battle_Squadron.sap",
		"Composers/Esquivel_Sal/Battle_Squadron_Stereo.sap",
		"Composers/Esquivel_Sal/Blaster_Master_Level_1.sap",
		"Composers/Esquivel_Sal/Blaster_Master_Level_1_Ste.sap",
		"Composers/Esquivel_Sal/Crocketts_Theme.sap",
		"Composers/Esquivel_Sal/Every_Breath_You_Take.sap",
		"Composers/Esquivel_Sal/Gemini_Man.sap",
		"Composers/Esquivel_Sal/Hybris_Title.sap",
		"Composers/Esquivel_Sal/Mega_Man_III_Sparkman.sap",
		"Composers/Esquivel_Sal/Obliterator_Stereo.sap",
		"Composers/Esquivel_Sal/OutRun_Magical_Sound_Sho_S.sap",
		"Composers/Esquivel_Sal/OutRun_Magical_Sound_Showe.sap",
		"Composers/Esquivel_Sal/OutRun_Splash_Wave.sap",
		"Composers/Esquivel_Sal/OutRun_Splash_Wave_Stereo.sap",
		"Composers/Esquivel_Sal/Outrun_Passing_Breeze.sap",
		"Composers/Esquivel_Sal/Outrun_Passing_Breeze_Ster.sap",
		"Composers/Esquivel_Sal/Space_Harrier_Godarni.sap",
		"Composers/Esquivel_Sal/Space_Harrier_Lakeside_Mem.sap",
		"Composers/Esquivel_Sal/Space_Harrier_White_Summer.sap",
		"Composers/Esquivel_Sal/Space_Harrier_Wiwi_Jumbo.sap",
		"Composers/Esquivel_Sal/Stardust_Memories.sap",
		"Composers/Esquivel_Sal/Tempest_Xtreem_Song_One.sap",
		"Composers/Esquivel_Sal/Tempest_Xtreem_Song_Three.sap",
		"Composers/Esquivel_Sal/Tempest_Xtreem_Song_Two.sap",
		"Composers/Esquivel_Sal/Tempest_Xtreem_Title_Song.sap",
		"Composers/Esquivel_Sal/Toobin.sap",
		"Composers/Esquivel_Sal/Toobin_Title.sap",
		"Composers/Esquivel_Sal/Trans_Atlantic.sap",
		"Composers/Ezcan_Kemal/Digiloo_Digiley.sap",
		"Composers/Ezcan_Kemal/Globetrotter.sap",
		"Composers/Farkas_Felker/ABBUC_36.sap",
		"Composers/Farkas_Felker/Joyride_End.sap",
		"Composers/Farkas_Felker/Joyride_Juggler.sap",
		"Composers/Farkas_Felker/Joyride_Landscape.sap",
		"Composers/Farkas_Felker/Joyride_Scrolls.sap",
		"Composers/Farkas_Felker/Joyride_Turn_Disk.sap",
		"Composers/Farkas_Felker/Joyride_Twirl.sap",
		"Composers/Feske_Nils/Tambumbi.sap",
		"Composers/Feske_Nils/Twista.sap",
		"Composers/Galinski_Adrian/Demology_2.sap",
		"Composers/Gilbertson_Gary/Alternate_Reality_City_Dan.sap",
		"Composers/Gilbertson_Gary/Alternate_Reality_City_Int.sap",
		"Composers/Gilbertson_Gary/Alternate_Reality_City_Smi.sap",
		"Composers/Gilmore_Adam/Zybex.sap",
		"Composers/Godlewski_Artur/Sea.sap",
		"Composers/Goff_Mark/Me_and_Ui_Jumping_in_the_S.sap",
		"Composers/Goff_Mark/crab_in_a_cave_final_boss.sap",
		"Composers/Goff_Mark/seaweed_cipst_underwater.sap",
		"Composers/Golewski_Filip/Another_Speedtro.sap",
		"Composers/Golewski_Filip/Big_Zine_1_Greetings.sap",
		"Composers/Golewski_Filip/Black.sap",
		"Composers/Golewski_Filip/Crazy_Magazine_1_2.sap",
		"Composers/Golewski_Filip/Date.sap",
		"Composers/Golewski_Filip/Future.sap",
		"Composers/Golewski_Filip/Heal.sap",
		"Composers/Golewski_Filip/Label.sap",
		"Composers/Golewski_Filip/SFW.sap",
		"Composers/Golewski_Filip/Story_Module.sap",
		"Composers/Golewski_Filip/TRC.sap",
		"Composers/Golewski_Filip/Tekkno.sap",
		"Composers/Golewski_Filip/Walenie_W_Stolek.sap",
		"Composers/Grad_Jacek/Grom.sap",
		"Composers/Grad_Jacek/Twarze_i_Cienie.sap",
		"Composers/Grayscale/Chromaluma.sap",
		"Composers/Grayscale/DrillDance.sap",
		"Composers/Grayscale/Gray_Set_Willy.sap",
		"Composers/Grayscale/Irish_Wood.sap",
		"Composers/Grayscale/Jam_Session.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Alive.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Alone.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Back_To_Life_2.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Cause_It_Hurts.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Czubeck_Parejd.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Deep_Kick.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Detox.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Expectancy.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Expression_2.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Extreme.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Extremely_Flammable.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/For_Replay.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Heartbreaker.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Hip.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Hope_1999.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Misunderstanding.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/One_Little_Step.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Poor_Rock_n_Roll.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Reborn.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Sabbath_2.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Sadness.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Schizofrenia.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Short_Reflex.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Slim_to_none.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/So_Alone.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Soul_Breakin.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Szczur.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Szwindel.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Total_Eclipse.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Transmission_Into_Your_Hea.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Tribute_to.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Ultra.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Ultrance.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/UnderTrencinPressure.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Voice_of_Silence_III_Previ.sap",
		"Composers/Grayscale/Kwiatek_Grzegorz/Voice_of_Silence_IV.sap",
		"Composers/Grayscale/Pneumatic_Driller.sap",
		"Composers/Grayscale/Running_Emu.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/ABC_Ingame.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Accident.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Andal.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Barymag_2_5.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Beer_Bibber.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Boremloza.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Cabil.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Czarna_dziura_w_dupie.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Dziobaty.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Falex.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Genia.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Glikol.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Happy_Oriental_Kids.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Kasz.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Klita.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Kowip.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Mepss.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Mixer.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Porazka_3_KamiBa.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Rake_in_Church.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Smell_Illusion.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Swift_Ray.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/Time_Is_Get_Rid_of_Grandfa.sap",
		"Composers/Grayscale/Sychowicz_Lukasz/X_Ray_2.sap",
		"Composers/Grayscale/Wasiel_Bartek/Dropsy_King.sap",
		"Composers/Grayscale/Wasiel_Bartek/Too_Late.sap",
		"Composers/Grayscale/Windy_Mind.sap",
		"Composers/Grayscale/WooBooDoo.sap",
		"Composers/Hay_Adam/Amegas.sap",
		"Composers/Hay_Adam/Approachong.sap",
		"Composers/Hay_Adam/Gyges_Will_You_Help_Me.sap",
		"Composers/Hay_Adam/Hawkeye.sap",
		"Composers/Hay_Adam/Psilocybe_Mexicana.sap",
		"Composers/Hay_Adam/Reaxion_Title.sap",
		"Composers/Hay_Adam/Wiz.sap",
		"Composers/Holda_Leszek/Klatwa_Ingame.sap",
		"Composers/Hryn_Piotr/Party_5.sap",
		"Composers/Hubbard_Rob/Extirpator.sap",
		"Composers/Hubbard_Rob/International_Karate.sap",
		"Composers/Hubbard_Rob/Jet_Set_Willy.sap",
		"Composers/Hubbard_Rob/Warhawk.sap",
		"Composers/Hudak_Matej/Shorty_Noises.sap",
		"Composers/Huntington_Glenys/Capt_Stickys_Gold.sap",
		"Composers/Huntington_Glenys/Fire_Chief.sap",
		"Composers/Huntington_Glenys/Robin_Hood.sap",
		"Composers/Husak_Jakub/Problem_Jasia_Ingame.sap",
		"Composers/Husak_Jakub/Rycerz.sap",
		"Composers/Husak_Jakub/Thinker_1.sap",
		"Composers/Iwaszko_Krystian/Droga_do_Duplandu.sap",
		"Composers/Iwaszko_Krystian/Dziad_sie_jara.sap",
		"Composers/Iwaszko_Krystian/Every_Bone_Broken.sap",
		"Composers/Iwaszko_Krystian/Hooy_F.sap",
		"Composers/Iwaszko_Krystian/IK_Crazy_Remix.sap",
		"Composers/Iwaszko_Krystian/Konduktor_Przyjacielem_Jes.sap",
		"Composers/Iwaszko_Krystian/Mechanic.sap",
		"Composers/Iwaszko_Krystian/No_Name.sap",
		"Composers/Iwaszko_Krystian/Pies_drapie_sie_po_jajach.sap",
		"Composers/Iwaszko_Krystian/R2K440.sap",
		"Composers/Iwaszko_Krystian/Samantha.sap",
		"Composers/Iwaszko_Krystian/Satan_Huj.sap",
		"Composers/Iwaszko_Krystian/Satan_Rox.sap",
		"Composers/Iwaszko_Krystian/THC_Kokot.sap",
		"Composers/Iwaszko_Krystian/THC_Rulla.sap",
		"Composers/Iwaszko_Krystian/THC_TMC.sap",
		"Composers/Iwaszko_Krystian/XC12.sap",
		"Composers/Iwaszko_Krystian/Z53.sap",
		"Composers/Iwaszko_Krystian/Zielony_Naplet.sap",
		"Composers/Jastrzemski_Marcin/Czdkom_2.sap",
		"Composers/Jastrzemski_Marcin/Fox.sap",
		"Composers/Johnson_Steve/Enemy_Within.sap",
		"Composers/Kalinowski_Bartosz/Barymag_1_Intro.sap",
		"Composers/Kalinowski_Bartosz/Hcl5.sap",
		"Composers/Kalinowski_Bartosz/Kapitan_Planeta.sap",
		"Composers/Kalinowski_Bartosz/Robin_Wojownik_Czasu.sap",
		"Composers/Kalinowski_Bartosz/Self_Test_2.sap",
		"Composers/Kalinowski_Bartosz/Sweet_Illusions_Shadebobs.sap",
		"Composers/Kalinowski_Bartosz/Zepi.sap",
		"Composers/Kalinowski_Bartosz/Zeus.sap",
		"Composers/Karpowicz_Henryk/Koleda_1.sap",
		"Composers/Karwacki_Jakub/East_Party_2003_Invi_Intro.sap",
		"Composers/Karwacki_Jakub/Lanos.sap",
		"Composers/Karwacki_Jakub/Lark.sap",
		"Composers/Karwacki_Jakub/Qwerty.sap",
		"Composers/Karwoth_Torsten/Demo_1.sap",
		"Composers/Karwoth_Torsten/FOFT.sap",
		"Composers/Kazimierczak_Leszek/Bad_Girl.sap",
		"Composers/Kazimierczak_Leszek/Megata_Zine_1_Intro.sap",
		"Composers/Kidaj_Andrzej/Minier.sap",
		"Composers/Kidaj_Andrzej/Yamaha.sap",
		"Composers/Kociuba_Dariusz/Odezwa_3_Lobo.sap",
		"Composers/Kociuba_Dariusz/Salcesonfix_Zine.sap",
		"Composers/Kociuba_Dariusz/Vioris.sap",
		"Composers/Kolakowski_Daniel/Mozart_01.sap",
		"Composers/Kolakowski_Daniel/Unite.sap",
		"Composers/Krawczyk_Adam/Magic_Dimension_1.sap",
		"Composers/Krix_Mario/Breakthrung.sap",
		"Composers/Krix_Mario/Complex.sap",
		"Composers/Krix_Mario/Dragon_Fine.sap",
		"Composers/Krix_Mario/Panther.sap",
		"Composers/Krix_Mario/Satisfaction.sap",
		"Composers/Krix_Mario/Sll1.sap",
		"Composers/Krix_Mario/Slow.sap",
		"Composers/Krix_Mario/Stardust_Memories.sap",
		"Composers/Krix_Mario/TT.sap",
		"Composers/Krix_Mario/Zeldahm.sap",
		"Composers/Kucharski_Konrad/Piczka.sap",
		"Composers/Kucisz_Tomasz/Network.sap",
		"Composers/Kuczek_Ireneusz/Echo.sap",
		"Composers/Kuczek_Ireneusz/Mama.sap",
		"Composers/Kuczek_Ireneusz/Said_I_Love_You.sap",
		"Composers/Kulinski_Grzegorz/Psy_hoza.sap",
		"Composers/Kulinski_Grzegorz/Yeah.sap",
		"Composers/Lajciak_Pavol/R_Type.sap",
		"Composers/Lajciak_Pavol/This_One_Comes_from_Slovak.sap",
		"Composers/Lajciak_Pavol/Wild_Jet.sap",
		"Composers/Lepkowski_Michal/Blackout.sap",
		"Composers/Lepkowski_Michal/Cypress.sap",
		"Composers/Lepkowski_Michal/Likuit_4k.sap",
		"Composers/Lepkowski_Michal/Odyssey_of_the_Mind.sap",
		"Composers/Lepkowski_Michal/Tekblast_Ingame_2.sap",
		"Composers/Lepkowski_Michal/Vengeance_Main_Part.sap",
		"Composers/Lewandowski_Marcin/Grunwald_1410_Ingame_2.sap",
		"Composers/Libic_Ladislav/Lac_Intro_II_part_6.sap",
		"Composers/Liebich_Tomasz/Alien.sap",
		"Composers/Liebich_Tomasz/Dictum_Acerbum.sap",
		"Composers/Liebich_Tomasz/Drack.sap",
		"Composers/Liebich_Tomasz/Duksap_1.sap",
		"Composers/Liebich_Tomasz/Fluid_Kha_Intro.sap",
		"Composers/Liebich_Tomasz/Hawkmoon.sap",
		"Composers/Liebich_Tomasz/Nemesis_2.sap",
		"Composers/Liebich_Tomasz/Perestroyka_Left_Channel.sap",
		"Composers/Liebich_Tomasz/Perestroyka_Right_Channel.sap",
		"Composers/Liebich_Tomasz/Sky_Computer_Network.sap",
		"Composers/Liebich_Tomasz/Technus.sap",
		"Composers/Liebich_Tomasz/Ucieczka.sap",
		"Composers/Lis_Marcin/Pyramid_Ingame.sap",
		"Composers/Lis_Piotr/Wyjasnijmy_to_sobie.sap",
		"Composers/Luberda_Michal/Barahir_Ingame.sap",
		"Composers/Luberda_Michal/Frank_and_Mark_Ingame.sap",
		"Composers/Luberda_Michal/Kernaw.sap",
		"Composers/Luberda_Michal/Kernaw_Ingame.sap",
		"Composers/Luberda_Michal/U235.sap",
		"Composers/Maderanek_Peter/Avex_1.sap",
		"Composers/Majchrzak_Marcin/D_City.sap",
		"Composers/Majchrzak_Marcin/Faza_4_Mona.sap",
		"Composers/Majchrzak_Marcin/Psychodela.sap",
		"Composers/Majchrzak_Marcin/Techno_Collection_4.sap",
		"Composers/Majewski_Tomasz/Copy_Party_1996_Info.sap",
		"Composers/Majewski_Tomasz/Detonator_Title.sap",
		"Composers/Majewski_Tomasz/Detonator_Winner.sap",
		"Composers/Majewski_Tomasz/Hellgate.sap",
		"Composers/Majewski_Tomasz/Lunapark.sap",
		"Composers/Majewski_Tomasz/Ninja.sap",
		"Composers/Majewski_Tomasz/Outsider.sap",
		"Composers/Majewski_Tomasz/Riders_From_The_Stars.sap",
		"Composers/Majewski_Tomasz/The_Truth_Is_Out_There.sap",
		"Composers/Maris_John/Mines.sap",
		"Composers/Martin_Aleksander/Bobsland.sap",
		"Composers/Martin_Aleksander/Kit_14.sap",
		"Composers/Martin_Aleksander/Max_Ingame.sap",
		"Composers/Martin_Aleksander/More.sap",
		"Composers/Martin_Aleksander/Mr_Proper.sap",
		"Composers/Martin_Aleksander/Mr_Proper_Ingame.sap",
		"Composers/Martin_Aleksander/Mr_Proper_Ingame_2.sap",
		"Composers/Martin_Aleksander/Sheol_side_A.sap",
		"Composers/Martin_Aleksander/Timeless_Announcemt_Load_5.sap",
		"Composers/Martin_Aleksander/Unknown.sap",
		"Composers/Martin_Chris/After_the_Storm.sap",
		"Composers/Martin_Chris/Approachong.sap",
		"Composers/Martin_Chris/Approachong_stereo.sap",
		"Composers/Martin_Chris/Blue_Monday.sap",
		"Composers/Martin_Chris/Crystal_Hammer.sap",
		"Composers/Martin_Chris/Cybernoid_Song.sap",
		"Composers/Martin_Chris/Deflector.sap",
		"Composers/Martin_Chris/Ecstacy_Song.sap",
		"Composers/Martin_Chris/Fairlightning.sap",
		"Composers/Martin_Chris/Popcorn_Song.sap",
		"Composers/Martin_Chris/Push_Maxi_Re_Mix.sap",
		"Composers/Matoga_Adrian/Meryb.sap",
		"Composers/Matoga_Adrian/Scream.sap",
		"Composers/Matyasik_Krzysztof/Atak_serca.sap",
		"Composers/Matyasik_Krzysztof/Karkoweczka.sap",
		"Composers/Matyasik_Krzysztof/Koninka.sap",
		"Composers/Matyasik_Krzysztof/Ogluszacz_Intro.sap",
		"Composers/Matyasik_Krzysztof/Pretty_Chlebek.sap",
		"Composers/Matyasik_Krzysztof/Schabik.sap",
		"Composers/Matyasik_Krzysztof/Wieprzowinka.sap",
		"Composers/Matyasik_Krzysztof/Wolowinka.sap",
		"Composers/Matyasik_Krzysztof/Wolowinka_Sampled.sap",
		"Composers/Mikolajczak_Pawel/Zbir.sap",
		"Composers/Mrozowski_Robert/Going_to_California.sap",
		"Composers/Munns_Richard/Black_Lamp.sap",
		"Composers/Munns_Richard/Black_Lamp_Ingame.sap",
		"Composers/Munns_Richard/Crumbles_Crisis.sap",
		"Composers/Munns_Richard/Filthy_Rich.sap",
		"Composers/Munns_Richard/Plastron.sap",
		"Composers/Munns_Richard/Rebound.sap",
		"Composers/Munns_Richard/Tagalon.sap",
		"Composers/Munns_Richard/Tube_Baddies.sap",
		"Composers/Munns_Richard/Zero_War.sap",
		"Composers/Murray_Chris/Winter_Games_88_Biathlon.sap",
		"Composers/Murray_Chris/Winter_Games_88_Bobsled.sap",
		"Composers/Murray_Chris/Winter_Games_88_Downhill.sap",
		"Composers/Murray_Chris/Winter_Games_88_Ski_Jump.sap",
		"Composers/Murray_Chris/Winter_Games_88_Slalom.sap",
		"Composers/Nedved_Vojtech/Amelie.sap",
		"Composers/Nedved_Vojtech/Aoki.sap",
		"Composers/Nedved_Vojtech/Rudi_Extrudi_Goes_to_Pragu.sap",
		"Composers/Numan_Daniel/Dwuetyloamid.sap",
		"Composers/Numan_Daniel/Komercja.sap",
		"Composers/Numan_Daniel/Motherfuckin_Bitch_Wodecka.sap",
		"Composers/Numan_Daniel/Pentagram_2_Info.sap",
		"Composers/Numan_Daniel/Pentagram_3_Music_1.sap",
		"Composers/Numan_Daniel/Pentagram_3_Music_3.sap",
		"Composers/Numan_Daniel/Pentagram_4_Silence.sap",
		"Composers/Numan_Daniel/Psychodelic_Acied.sap",
		"Composers/Numan_Daniel/Raving_Vieprz_1.sap",
		"Composers/Numan_Daniel/THC_Vibration.sap",
		"Composers/Numan_Daniel/Vasco_Public_Domain_10_3.sap",
		"Composers/Oglodek_Marek/Odsyfka.sap",
		"Composers/Oscadal_Filip/Balleantro.sap",
		"Composers/Oscadal_Filip/Brutal_Recall_Demo.sap",
		"Composers/Oscadal_Filip/Lemmings_Intro.sap",
		"Composers/Oscadal_Filip/Lemmings_Intro_2.sap",
		"Composers/Oscadal_Filip/Vaxeen_Photographic.sap",
		"Composers/Padula_Jaroslaw/Horror.sap",
		"Composers/Padula_Jaroslaw/Keczapek.sap",
		"Composers/Padula_Jaroslaw/Men_02.sap",
		"Composers/Padula_Jaroslaw/Przepraszam.sap",
		"Composers/Padula_Jaroslaw/Reason.sap",
		"Composers/Padula_Jaroslaw/So_Old.sap",
		"Composers/Padula_Jaroslaw/Some_Nice_AB.sap",
		"Composers/Padula_Jaroslaw/Tech2.sap",
		"Composers/Padula_Jaroslaw/Techno_Collection_1.sap",
		"Composers/Padula_Jaroslaw/Toy_Intro.sap",
		"Composers/Padula_Jaroslaw/Very_Very.sap",
		"Composers/Padula_Jaroslaw/Wave_X.sap",
		"Composers/Padula_Jaroslaw/Xc9012.sap",
		"Composers/Padula_Jaroslaw/Z.sap",
		"Composers/Panopticum/Atari.sap",
		"Composers/Panopticum/Autumn.sap",
		"Composers/Panopticum/Bonanza.sap",
		"Composers/Panopticum/Guns.sap",
		"Composers/Panopticum/Morze.sap",
		"Composers/Panopticum/Motor.sap",
		"Composers/Panopticum/Musiczka.sap",
		"Composers/Panopticum/Summer.sap",
		"Composers/Pasiecznik_Michal/Damned.sap",
		"Composers/Pasiecznik_Michal/Damned_Remix.sap",
		"Composers/Pelc_Janusz/Captain_Gather.sap",
		"Composers/Pelc_Janusz/Lasermania.sap",
		"Composers/Pelc_Janusz/Winner.sap",
		"Composers/Pesout_Marek/Acid_1.sap",
		"Composers/Pesout_Marek/Anger.sap",
		"Composers/Pesout_Marek/Asylum.sap",
		"Composers/Pesout_Marek/Bad_Drome.sap",
		"Composers/Pesout_Marek/Blade_Runner.sap",
		"Composers/Pesout_Marek/Bounce.sap",
		"Composers/Pesout_Marek/Czech_Sound_2.sap",
		"Composers/Pesout_Marek/Delirium.sap",
		"Composers/Pesout_Marek/Destruction.sap",
		"Composers/Pesout_Marek/Dick_Tracy.sap",
		"Composers/Pesout_Marek/Gangsters.sap",
		"Composers/Pesout_Marek/Genlog.sap",
		"Composers/Pesout_Marek/Lemmblaster.sap",
		"Composers/Pesout_Marek/Liftboy.sap",
		"Composers/Pesout_Marek/Megamix.sap",
		"Composers/Pesout_Marek/Messiah_from_Outer_Space.sap",
		"Composers/Pesout_Marek/Molecula.sap",
		"Composers/Pesout_Marek/Paegas.sap",
		"Composers/Pesout_Marek/Paradise.sap",
		"Composers/Pesout_Marek/Peach.sap",
		"Composers/Pesout_Marek/Picture_H_I_P.sap",
		"Composers/Pesout_Marek/Science.sap",
		"Composers/Pesout_Marek/Shadow.sap",
		"Composers/Pesout_Marek/Swamp_Thing.sap",
		"Composers/Pesout_Marek/T_Rex.sap",
		"Composers/Pesout_Marek/Techwalk.sap",
		"Composers/Pesout_Marek/Tekk_Drums_1.sap",
		"Composers/Pesout_Marek/Tekkno_Experience_part_2_1.sap",
		"Composers/Pesout_Marek/Tekkno_Experience_part_2_2.sap",
		"Composers/Pesout_Marek/Ticket.sap",
		"Composers/Pesout_Marek/Tribal.sap",
		"Composers/Pesout_Marek/Who_3.sap",
		"Composers/Piscol_Juergen/Atari_Variationen.sap",
		"Composers/Piscol_Juergen/Synthi_1.sap",
		"Composers/Podedworny_Wojtek/Pandemonium.sap",
		"Composers/Podedworny_Wojtek/Wizard.sap",
		"Composers/Potter_Mike/Chicken.sap",
		"Composers/Potter_Mike/Plaque_Man.sap",
		"Composers/Potter_Mike/Protector.sap",
		"Composers/Potter_Mike/Protector_II.sap",
		"Composers/Potter_Mike/Shadow_World.sap",
		"Composers/Przybyszewski_Krzysztof/Blasta_Basta.sap",
		"Composers/Przybyszewski_Krzysztof/Darkness_Subway.sap",
		"Composers/Przybyszewski_Krzysztof/Fast_Excess.sap",
		"Composers/Przybyszewski_Krzysztof/Happy_97.sap",
		"Composers/Przybyszewski_Krzysztof/Lama.sap",
		"Composers/Przybyszewski_Krzysztof/Lokowka_Szal.sap",
		"Composers/Przybyszewski_Krzysztof/Place_in_the_Space.sap",
		"Composers/Przybyszewski_Krzysztof/Room.sap",
		"Composers/Przybyszewski_Krzysztof/Siphon.sap",
		"Composers/Przybyszewski_Krzysztof/Spawik.sap",
		"Composers/Pucinski_Rafal/AR_1.sap",
		"Composers/Pucinski_Rafal/Anty_SS_Zine_2.sap",
		"Composers/Pucinski_Rafal/Disk.sap",
		"Composers/Pucinski_Rafal/Memory.sap",
		"Composers/Pucinski_Rafal/Warrior.sap",
		"Composers/Radecki_Michal/Ilusia.sap",
		"Composers/Radecki_Michal/Kostka.sap",
		"Composers/Radzikowski_Ireneusz/Another_Day_in_Paradise.sap",
		"Composers/Radzikowski_Ireneusz/Back_to_Life.sap",
		"Composers/Radzikowski_Ireneusz/Christmas_mix.sap",
		"Composers/Radzikowski_Ireneusz/Comeback.sap",
		"Composers/Radzikowski_Ireneusz/Cyper_Space.sap",
		"Composers/Radzikowski_Ireneusz/Draconus_Remix.sap",
		"Composers/Radzikowski_Ireneusz/Escape_from_the_Orbit.sap",
		"Composers/Radzikowski_Ireneusz/Historia_Niespelnionych_4.sap",
		"Composers/Radzikowski_Ireneusz/Puzmania.sap",
		"Composers/Radzikowski_Ireneusz/Savage_Intro.sap",
		"Composers/Radzikowski_Ireneusz/The_Best_Car.sap",
		"Composers/Rentgen/Victoria_JHS_Hidden_1.sap",
		"Composers/Roemer_Markus/Biene_Maja.sap",
		"Composers/Rymorz_W/Arkanoid_II.sap",
		"Composers/Scarim_Nicholas/Spy_vs_Spy_Arctic_Antics.sap",
		"Composers/Skwiot_Stanislaw/Fortuna.sap",
		"Composers/Stanik_Krzysztof/Ble_ble_ble.sap",
		"Composers/Stanik_Krzysztof/Mroczny.sap",
		"Composers/Sterba_Radek/Android.sap",
		"Composers/Sterba_Radek/Astro4road.sap",
		"Composers/Sterba_Radek/Basix.sap",
		"Composers/Sterba_Radek/Cervi2.sap",
		"Composers/Sterba_Radek/Cubico.sap",
		"Composers/Sterba_Radek/Enigma_part_2.sap",
		"Composers/Sterba_Radek/First.sap",
		"Composers/Sterba_Radek/Flop_Magazin_30_Just_Now.sap",
		"Composers/Sterba_Radek/Flop_Magazin_31.sap",
		"Composers/Sterba_Radek/Flop_Magazin_31_Christmas.sap",
		"Composers/Sterba_Radek/Flop_Magazin_33.sap",
		"Composers/Sterba_Radek/Flop_Magazin_36_38.sap",
		"Composers/Sterba_Radek/Flop_Magazin_40.sap",
		"Composers/Sterba_Radek/Gem_x.sap",
		"Composers/Sterba_Radek/Illusion_2.sap",
		"Composers/Sterba_Radek/Indiana_Jones_4.sap",
		"Composers/Sterba_Radek/L45t_M1nut3.sap",
		"Composers/Sterba_Radek/Music.sap",
		"Composers/Sterba_Radek/Naturix_Tune_2.sap",
		"Composers/Sterba_Radek/Posthelper.sap",
		"Composers/Sterba_Radek/Posthelper_Tune_3.sap",
		"Composers/Sterba_Radek/Predator.sap",
		"Composers/Strobe/Romantic_Squirrelwedding_i.sap",
		"Composers/Strobe/Star_Legend_Mainscreen_Int.sap",
		"Composers/Strobe/Town_of_Ui.sap",
		"Composers/Strzelec_Pawel/Miny.sap",
		"Composers/Szczesniak_Konrad/3d24_1.sap",
		"Composers/Szczesniak_Konrad/3d24_2.sap",
		"Composers/Szczesniak_Maciek/Das_Anty_Adolf_Demo_1.sap",
		"Composers/Szpilowski_Michal/7_Gates_of_Jambala.sap",
		"Composers/Szpilowski_Michal/Bomb_Jack.sap",
		"Composers/Szpilowski_Michal/Cherry_Rave_Experimental.sap",
		"Composers/Szpilowski_Michal/Das_Boot_X.sap",
		"Composers/Szpilowski_Michal/Docent.sap",
		"Composers/Szpilowski_Michal/Galaxia_Ingame.sap",
		"Composers/Szpilowski_Michal/Game_Over.sap",
		"Composers/Szpilowski_Michal/Isora.sap",
		"Composers/Szpilowski_Michal/Kaktus.sap",
		"Composers/Szpilowski_Michal/Mayhem_X.sap",
		"Composers/Szpilowski_Michal/Moscow_1993_Stage_1.sap",
		"Composers/Szpilowski_Michal/My_First_One_RMT.sap",
		"Composers/Szpilowski_Michal/Nexus_Ingame_2.sap",
		"Composers/Szpilowski_Michal/Ocean_Detox.sap",
		"Composers/Szpilowski_Michal/Poseidon.sap",
		"Composers/Szpilowski_Michal/Postcard_from_Middle_East.sap",
		"Composers/Szpilowski_Michal/Rondell.sap",
		"Composers/Szpilowski_Michal/Serious_Magazine_3_4.sap",
		"Composers/Szpilowski_Michal/Sexy_Six_Alfa_Title.sap",
		"Composers/Szpilowski_Michal/Some_Song_Remixed.sap",
		"Composers/Szpilowski_Michal/Song_12.sap",
		"Composers/Szpilowski_Michal/Teacher_Killer.sap",
		"Composers/Szpilowski_Michal/Techdrum.sap",
		"Composers/Szpilowski_Michal/Unknown_Game.sap",
		"Composers/Szpilowski_Michal/X_Demo_Main_Part.sap",
		"Composers/Szpilowski_Michal/X_Mass_Greetings_Part.sap",
		"Composers/Szpilowski_Michal/Yie_Ar_Kung_Fu.sap",
		"Composers/Szpilowski_Michal/rAmols_Revenge.sap",
		"Composers/Szymczuk_Daniel/Loriens_Tomb_Game_Over.sap",
		"Composers/Takacs_Gabriel/Cappela.sap",
		"Composers/Takacs_Gabriel/Trash.sap",
		"Composers/Tegethoff_Sven/Roller.sap",
		"Composers/Tegethoff_Sven/The_Top_2_2.sap",
		"Composers/Ui/3_Joonkiz_Skullz.sap",
		"Composers/Ui/Los_Osos_hablan_en_Marte.sap",
		"Composers/Verdaasdonk_Robert/Unconventional_2000.sap",
		"Composers/Vila_Jaime/Gyruss_Stereo.sap",
		"Composers/Vila_Jaime/Megamix_Atari.sap",
		"Composers/Vila_Jaime/Short_Song.sap",
		"Composers/Vogt_Dariusz/DJ_V_4.sap",
		"Composers/Vogt_Dariusz/DJ_V_9.sap",
		"Composers/Vogt_Dariusz/Leiser_Intro.sap",
		"Composers/Vybostok_Marian/Alpha.sap",
		"Composers/Vybostok_Marian/Bristly_Hedgehog.sap",
		"Composers/Vybostok_Marian/Bristly_Hedgehog_Megamix.sap",
		"Composers/Vybostok_Marian/Wanted_Part_1.sap",
		"Composers/Vybostok_Marian/Wanted_Part_3_1.sap",
		"Composers/Vybostok_Marian/Wanted_Part_4_Mix.sap",
		"Composers/Vybostok_Marian/Wanted_Part_4_Zeus_Mix.sap",
		"Composers/Wasilewski_Dariusz/Daras.sap",
		"Composers/Wasilewski_Dariusz/Labirynt_Smierci.sap",
		"Composers/Whittaker_David/Amaurote.sap",
		"Composers/Whittaker_David/Grand_Prix_Simulator.sap",
		"Composers/Whittaker_David/Panther.sap",
		"Composers/Whittaker_David/Red_Max.sap",
		"Composers/Whittaker_David/Trans_Muter.sap",
		"Composers/Wisniewski_Mateusz/High_Tide.sap",
		"Composers/Wisniewski_Mateusz/Sillyventure_2k4_Inv.sap",
		"Composers/Witkiewicz_Dariusz/Dragon.sap",
		"Composers/Witkiewicz_Dariusz/Grass_Slideshow_End.sap",
		"Games/Alien_Ambush.sap",
		"Games/Andromeda.sap",
		"Games/Archon.sap",
		"Games/Astro_Chase.sap",
		"Games/Atomia.sap",
		"Games/Bilbo.sap",
		"Games/Boulder_Dash_2.sap",
		"Games/Bristles.sap",
		"Games/Carnival_Massacre.sap",
		"Games/Caverns_of_Khafka.sap",
		"Games/Chimera.sap",
		"Games/Clowns_and_Balloons.sap",
		"Games/Conan.sap",
		"Games/Culmins_1.sap",
		"Games/Culmins_2.sap",
		"Games/Dan_Strikes_Back.sap",
		"Games/Darts.sap",
		"Games/Diamonds.sap",
		"Games/Elevator_Repairman.sap",
		"Games/Emil_Hilf.sap",
		"Games/Fire_Power.sap",
		"Games/Flip_and_Flop.sap",
		"Games/Floor_Walker.sap",
		"Games/Frogger.sap",
		"Games/Frogger_II.sap",
		"Games/Ghost_Chaser.sap",
		"Games/Groove.sap",
		"Games/Gunfight.sap",
		"Games/Gyruss.sap",
		"Games/Herbert.sap",
		"Games/Jigsaws.sap",
		"Games/Jumping_Jacks_Big_Adventur.sap",
		"Games/Kasiarz.sap",
		"Games/Keystone_Kapers.sap",
		"Games/Killa_Cycle.sap",
		"Games/Loaded_Brain.sap",
		"Games/Loops_DX.sap",
		"Games/Major_Bronx.sap",
		"Games/Mario_Bros.sap",
		"Games/Master_of_Time.sap",
		"Games/Master_of_the_Lamps.sap",
		"Games/Mediator.sap",
		"Games/Moon_Patrol.sap",
		"Games/Nadral.sap",
		"Games/Ninja.sap",
		"Games/On_Track.sap",
		"Games/Pac_Man.sap",
		"Games/Pac_Man_Eric_Wolz.sap",
		"Games/Pacific_Coast_Highway.sap",
		"Games/Poker_Sam.sap",
		"Games/Professor_IQ.sap",
		"Games/Saper.sap",
		"Games/Sargon_II.sap",
		"Games/Screaming_Wings.sap",
		"Games/Sea_Chase.sap",
		"Games/Shamus.sap",
		"Games/Shamus_Case_II.sap",
		"Games/Softoy.sap",
		"Games/Special_Delivery.sap",
		"Games/Spy_Games_Ingame.sap",
		"Games/Spys_Demise.sap",
		"Games/Star_Intruder.sap",
		"Games/Strip_Poker.sap",
		"Games/Submission.sap",
		"Games/Tapper.sap",
		"Games/Taxicab_Hill.sap",
		"Games/William_Tell.sap",
		"Misc/Astrosphere.sap",
		"Misc/Avex_Intro.sap",
		"Misc/Bajm.sap",
		"Misc/Banklan_2.sap",
		"Misc/Deadline.sap",
		"Misc/Debiut.sap",
		"Misc/Desease.sap",
		"Misc/Hobbytronic_89_1.sap",
		"Misc/Huhi.sap",
		"Misc/Instrumentarium.sap",
		"Misc/L_Orneta_Demo.sap",
		"Misc/Luzik.sap",
		"Misc/My_First_Pokey.sap",
		"Misc/New_Song_1D.sap",
		"Misc/Only_Chipchop_Can_Save_Us.sap",
		"Misc/Pokety_Bitches.sap",
		"Misc/Star_Shot.sap",
		"Misc/Tajemnice_Atari_Intro.sap",
		"Misc/Takie_Cos.sap",
		"Misc/The_Great_Commandment.sap",
		"Misc/Tribute_To_Virus.sap",
		"Unknown/AMS/Abracadabra.sap",
		"Unknown/AMS/Africa.sap",
		"Unknown/AMS/American_Pie.sap",
		"Unknown/AMS/Another_Brick_in_the_Wall.sap",
		"Unknown/AMS/Back_in_the_USSR.sap",
		"Unknown/AMS/Bee_Fly.sap",
		"Unknown/AMS/Blue_Moon.sap",
		"Unknown/AMS/Calypso.sap",
		"Unknown/AMS/Cancan.sap",
		"Unknown/AMS/Cecilia.sap",
		"Unknown/AMS/Copacabana.sap",
		"Unknown/AMS/Dallas.sap",
		"Unknown/AMS/Dancing_in_the_Dark.sap",
		"Unknown/AMS/Do_Ya_Think_I_am_Sexy.sap",
		"Unknown/AMS/Down_Under.sap",
		"Unknown/AMS/Eleanor_Rigby.sap",
		"Unknown/AMS/Footloose.sap",
		"Unknown/AMS/Hey_Jude.sap",
		"Unknown/AMS/I_Just_Call_to_Say_I_Love.sap",
		"Unknown/AMS/Imperial_March_2.sap",
		"Unknown/AMS/Jingle_Bells.sap",
		"Unknown/AMS/Light_My_Fire.sap",
		"Unknown/AMS/Lucy_in_the_Sky_with_Diamo.sap",
		"Unknown/AMS/Maniac.sap",
		"Unknown/AMS/Moonlight_Shadow.sap",
		"Unknown/AMS/Muppet_Show.sap",
		"Unknown/AMS/Ninety_Nine_Red_Balloons_2.sap",
		"Unknown/AMS/Norwegian_Wood.sap",
		"Unknown/AMS/Oh_My_Darling_Clementine.sap",
		"Unknown/AMS/Old_Batman_TV_Show.sap",
		"Unknown/AMS/Owner_of_the_Lonely_Heart.sap",
		"Unknown/AMS/Paperback_Writer.sap",
		"Unknown/AMS/Rosanna.sap",
		"Unknown/AMS/Say_Say_Say.sap",
		"Unknown/AMS/Sleigh_Ride.sap",
		"Unknown/AMS/Sleigh_Ride_2.sap",
		"Unknown/AMS/Smoke_on_the_Water.sap",
		"Unknown/AMS/Stairway_to_Heaven.sap",
		"Unknown/AMS/Star_Trek.sap",
		"Unknown/AMS/Star_Wars_Cantina_2.sap",
		"Unknown/AMS/Summer_Nights.sap",
		"Unknown/AMS/Take_Five.sap",
		"Unknown/AMS/Tie_a_Yellow_Ribbon_Round.sap",
		"Unknown/AMS/Total_Eclipse_of_the_Heart.sap",
		"Unknown/AMS/Tradition.sap",
		"Unknown/AMS/We_Can_Work_It_out.sap",
		"Unknown/AMS/When_Im_64.sap",
		"Unknown/AMS/Winds_of_War.sap",
		"Unknown/AMS/Yellow_Submarine.sap",
		"Unknown/Autum.sap",
		"Unknown/Blus.sap",
		"Unknown/CompyShop_Magazin_Inside_B.sap",
		"Unknown/Condor.sap",
		"Unknown/Dajana.sap",
		"Unknown/Door.sap",
		"Unknown/For_Lobo_2.sap",
		"Unknown/Grzyby.sap",
		"Unknown/High_Society_Girl.sap",
		"Unknown/House_of_Rising_Sun.sap",
		"Unknown/Lunatic.sap",
		"Unknown/Music1.sap",
		"Unknown/Music2.sap",
		"Unknown/NATO.sap",
		"Unknown/Panorama.sap",
		"Unknown/Ranger1.sap",
		"Unknown/Ranger7.sap",
		"Unknown/Ranger9.sap",
		"Unknown/Skladanka.sap",
		"Unknown/Techno.sap",
		"Unknown/Time_To_Enjoy.sap",
		"Unknown/Train.sap",
		"Unknown/Voyager.sap",
		"Unknown/Yeach.sap"
	};
}
