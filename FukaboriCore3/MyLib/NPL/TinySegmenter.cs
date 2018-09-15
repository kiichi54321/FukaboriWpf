﻿/*
 * 部分的辞書使用に改造。
 * (c) 2014 Takaichi Ito (@kiichi54321)
 * 
 * 元のC#版作者
 * TinySegmenter.NET 0.1.1 -- C# Version of TinySegmenter
 * (c) 2010 DOBON! <http://dobon.net>

 * Original JavaScript Version CopyRight Notice
// TinySegmenter 0.1 -- Super compact Japanese tokenizer in Javascript
// (c) 2008 Taku Kudo <taku@chasen.org>
// TinySegmenter is freely distributable under the terms of a new BSD licence.
// For details, see http://chasen.org/~taku/software/TinySegmenter/LICENCE.txt
*/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using RawlerLib.MyExtend;

namespace TinySegmenterDotNet
{
    public class TinySegmenter
    {
        #region constants
        private int BIAS__ = -332;
        private Dictionary<string, int> BC1__ = new Dictionary<string, int>();
        private Dictionary<string, int> BC2__ = new Dictionary<string, int>();
        private Dictionary<string, int> BC3__ = new Dictionary<string, int>();
        private Dictionary<string, int> BP1__ = new Dictionary<string, int>();
        private Dictionary<string, int> BP2__ = new Dictionary<string, int>();
        private Dictionary<string, int> BQ1__ = new Dictionary<string, int>();
        private Dictionary<string, int> BQ2__ = new Dictionary<string, int>();
        private Dictionary<string, int> BQ3__ = new Dictionary<string, int>();
        private Dictionary<string, int> BQ4__ = new Dictionary<string, int>();
        private Dictionary<string, int> BW1__ = new Dictionary<string, int>();
        private Dictionary<string, int> BW2__ = new Dictionary<string, int>();
        private Dictionary<string, int> BW3__ = new Dictionary<string, int>();
        private Dictionary<string, int> TC1__ = new Dictionary<string, int>();
        private Dictionary<string, int> TC2__ = new Dictionary<string, int>();
        private Dictionary<string, int> TC3__ = new Dictionary<string, int>();
        private Dictionary<string, int> TC4__ = new Dictionary<string, int>();
        private Dictionary<string, int> TQ1__ = new Dictionary<string, int>();
        private Dictionary<string, int> TQ2__ = new Dictionary<string, int>();
        private Dictionary<string, int> TQ3__ = new Dictionary<string, int>();
        private Dictionary<string, int> TQ4__ = new Dictionary<string, int>();
        private Dictionary<string, int> TW1__ = new Dictionary<string, int>();
        private Dictionary<string, int> TW2__ = new Dictionary<string, int>();
        private Dictionary<string, int> TW3__ = new Dictionary<string, int>();
        private Dictionary<string, int> TW4__ = new Dictionary<string, int>();
        private Dictionary<string, int> UC1__ = new Dictionary<string, int>();
        private Dictionary<string, int> UC2__ = new Dictionary<string, int>();
        private Dictionary<string, int> UC3__ = new Dictionary<string, int>();
        private Dictionary<string, int> UC4__ = new Dictionary<string, int>();
        private Dictionary<string, int> UC5__ = new Dictionary<string, int>();
        private Dictionary<string, int> UC6__ = new Dictionary<string, int>();
        private Dictionary<string, int> UP1__ = new Dictionary<string, int>();
        private Dictionary<string, int> UP2__ = new Dictionary<string, int>();
        private Dictionary<string, int> UP3__ = new Dictionary<string, int>();
        private Dictionary<string, int> UQ1__ = new Dictionary<string, int>();
        private Dictionary<string, int> UQ2__ = new Dictionary<string, int>();
        private Dictionary<string, int> UQ3__ = new Dictionary<string, int>();
        private Dictionary<string, int> UW1__ = new Dictionary<string, int>();
        private Dictionary<string, int> UW2__ = new Dictionary<string, int>();
        private Dictionary<string, int> UW3__ = new Dictionary<string, int>();
        private Dictionary<string, int> UW4__ = new Dictionary<string, int>();
        private Dictionary<string, int> UW5__ = new Dictionary<string, int>();
        private Dictionary<string, int> UW6__ = new Dictionary<string, int>();

        private void InitializeConstants()
        {
            BC1__.Add("HH", 6);
            BC1__.Add("II", 2461);
            BC1__.Add("KH", 406);
            BC1__.Add("OH", -1378);

            BC2__.Add("AA", -3267);
            BC2__.Add("AI", 2744);
            BC2__.Add("AN", -878);
            BC2__.Add("HH", -4070);
            BC2__.Add("HM", -1711);
            BC2__.Add("HN", 4012);
            BC2__.Add("HO", 3761);
            BC2__.Add("IA", 1327);
            BC2__.Add("IH", -1184);
            BC2__.Add("II", -1332);
            BC2__.Add("IK", 1721);
            BC2__.Add("IO", 5492);
            BC2__.Add("KI", 3831);
            BC2__.Add("KK", -8741);
            BC2__.Add("MH", -3132);
            BC2__.Add("MK", 3334);
            BC2__.Add("OO", -2920);

            BC3__.Add("HH", 996);
            BC3__.Add("HI", 626);
            BC3__.Add("HK", -721);
            BC3__.Add("HN", -1307);
            BC3__.Add("HO", -836);
            BC3__.Add("IH", -301);
            BC3__.Add("KK", 2762);
            BC3__.Add("MK", 1079);
            BC3__.Add("MM", 4034);
            BC3__.Add("OA", -1652);
            BC3__.Add("OH", 266);

            BP1__.Add("BB", 295);
            BP1__.Add("OB", 304);
            BP1__.Add("OO", -125);
            BP1__.Add("UB", 352);

            BP2__.Add("BO", 60);
            BP2__.Add("OO", -1762);

            BQ1__.Add("BHH", 1150);
            BQ1__.Add("BHM", 1521);
            BQ1__.Add("BII", -1158);
            BQ1__.Add("BIM", 886);
            BQ1__.Add("BMH", 1208);
            BQ1__.Add("BNH", 449);
            BQ1__.Add("BOH", -91);
            BQ1__.Add("BOO", -2597);
            BQ1__.Add("OHI", 451);
            BQ1__.Add("OIH", -296);
            BQ1__.Add("OKA", 1851);
            BQ1__.Add("OKH", -1020);
            BQ1__.Add("OKK", 904);
            BQ1__.Add("OOO", 2965);

            BQ2__.Add("BHH", 118);
            BQ2__.Add("BHI", -1159);
            BQ2__.Add("BHM", 466);
            BQ2__.Add("BIH", -919);
            BQ2__.Add("BKK", -1720);
            BQ2__.Add("BKO", 864);
            BQ2__.Add("OHH", -1139);
            BQ2__.Add("OHM", -181);
            BQ2__.Add("OIH", 153);
            BQ2__.Add("UHI", -1146);

            BQ3__.Add("BHH", -792);
            BQ3__.Add("BHI", 2664);
            BQ3__.Add("BII", -299);
            BQ3__.Add("BKI", 419);
            BQ3__.Add("BMH", 937);
            BQ3__.Add("BMM", 8335);
            BQ3__.Add("BNN", 998);
            BQ3__.Add("BOH", 775);
            BQ3__.Add("OHH", 2174);
            BQ3__.Add("OHM", 439);
            BQ3__.Add("OII", 280);
            BQ3__.Add("OKH", 1798);
            BQ3__.Add("OKI", -793);
            BQ3__.Add("OKO", -2242);
            BQ3__.Add("OMH", -2402);
            BQ3__.Add("OOO", 11699);

            BQ4__.Add("BHH", -3895);
            BQ4__.Add("BIH", 3761);
            BQ4__.Add("BII", -4654);
            BQ4__.Add("BIK", 1348);
            BQ4__.Add("BKK", -1806);
            BQ4__.Add("BMI", -3385);
            BQ4__.Add("BOO", -12396);
            BQ4__.Add("OAH", 926);
            BQ4__.Add("OHH", 266);
            BQ4__.Add("OHK", -2036);
            BQ4__.Add("ONN", -973);

            BW1__.Add(",と", 660);
            BW1__.Add(",同", 727);
            BW1__.Add("B1あ", 1404);
            BW1__.Add("B1同", 542);
            BW1__.Add("、と", 660);
            BW1__.Add("、同", 727);
            BW1__.Add("」と", 1682);
            BW1__.Add("あっ", 1505);
            BW1__.Add("いう", 1743);
            BW1__.Add("いっ", -2055);
            BW1__.Add("いる", 672);
            BW1__.Add("うし", -4817);
            BW1__.Add("うん", 665);
            BW1__.Add("から", 3472);
            BW1__.Add("がら", 600);
            BW1__.Add("こう", -790);
            BW1__.Add("こと", 2083);
            BW1__.Add("こん", -1262);
            BW1__.Add("さら", -4143);
            BW1__.Add("さん", 4573);
            BW1__.Add("した", 2641);
            BW1__.Add("して", 1104);
            BW1__.Add("すで", -3399);
            BW1__.Add("そこ", 1977);
            BW1__.Add("それ", -871);
            BW1__.Add("たち", 1122);
            BW1__.Add("ため", 601);
            BW1__.Add("った", 3463);
            BW1__.Add("つい", -802);
            BW1__.Add("てい", 805);
            BW1__.Add("てき", 1249);
            BW1__.Add("でき", 1127);
            BW1__.Add("です", 3445);
            BW1__.Add("では", 844);
            BW1__.Add("とい", -4915);
            BW1__.Add("とみ", 1922);
            BW1__.Add("どこ", 3887);
            BW1__.Add("ない", 5713);
            BW1__.Add("なっ", 3015);
            BW1__.Add("など", 7379);
            BW1__.Add("なん", -1113);
            BW1__.Add("にし", 2468);
            BW1__.Add("には", 1498);
            BW1__.Add("にも", 1671);
            BW1__.Add("に対", -912);
            BW1__.Add("の一", -501);
            BW1__.Add("の中", 741);
            BW1__.Add("ませ", 2448);
            BW1__.Add("まで", 1711);
            BW1__.Add("まま", 2600);
            BW1__.Add("まる", -2155);
            BW1__.Add("やむ", -1947);
            BW1__.Add("よっ", -2565);
            BW1__.Add("れた", 2369);
            BW1__.Add("れで", -913);
            BW1__.Add("をし", 1860);
            BW1__.Add("を見", 731);
            BW1__.Add("亡く", -1886);
            BW1__.Add("京都", 2558);
            BW1__.Add("取り", -2784);
            BW1__.Add("大き", -2604);
            BW1__.Add("大阪", 1497);
            BW1__.Add("平方", -2314);
            BW1__.Add("引き", -1336);
            BW1__.Add("日本", -195);
            BW1__.Add("本当", -2423);
            BW1__.Add("毎日", -2113);
            BW1__.Add("目指", -724);
            BW1__.Add("Ｂ１あ", 1404);
            BW1__.Add("Ｂ１同", 542);
            BW1__.Add("｣と", 1682);

            BW2__.Add("..", -11822);
            BW2__.Add("11", -669);
            BW2__.Add("――", -5730);
            BW2__.Add("−−", -13175);
            BW2__.Add("いう", -1609);
            BW2__.Add("うか", 2490);
            BW2__.Add("かし", -1350);
            BW2__.Add("かも", -602);
            BW2__.Add("から", -7194);
            BW2__.Add("かれ", 4612);
            BW2__.Add("がい", 853);
            BW2__.Add("がら", -3198);
            BW2__.Add("きた", 1941);
            BW2__.Add("くな", -1597);
            BW2__.Add("こと", -8392);
            BW2__.Add("この", -4193);
            BW2__.Add("させ", 4533);
            BW2__.Add("され", 13168);
            BW2__.Add("さん", -3977);
            BW2__.Add("しい", -1819);
            BW2__.Add("しか", -545);
            BW2__.Add("した", 5078);
            BW2__.Add("して", 972);
            BW2__.Add("しな", 939);
            BW2__.Add("その", -3744);
            BW2__.Add("たい", -1253);
            BW2__.Add("たた", -662);
            BW2__.Add("ただ", -3857);
            BW2__.Add("たち", -786);
            BW2__.Add("たと", 1224);
            BW2__.Add("たは", -939);
            BW2__.Add("った", 4589);
            BW2__.Add("って", 1647);
            BW2__.Add("っと", -2094);
            BW2__.Add("てい", 6144);
            BW2__.Add("てき", 3640);
            BW2__.Add("てく", 2551);
            BW2__.Add("ては", -3110);
            BW2__.Add("ても", -3065);
            BW2__.Add("でい", 2666);
            BW2__.Add("でき", -1528);
            BW2__.Add("でし", -3828);
            BW2__.Add("です", -4761);
            BW2__.Add("でも", -4203);
            BW2__.Add("とい", 1890);
            BW2__.Add("とこ", -1746);
            BW2__.Add("とと", -2279);
            BW2__.Add("との", 720);
            BW2__.Add("とみ", 5168);
            BW2__.Add("とも", -3941);
            BW2__.Add("ない", -2488);
            BW2__.Add("なが", -1313);
            BW2__.Add("など", -6509);
            BW2__.Add("なの", 2614);
            BW2__.Add("なん", 3099);
            BW2__.Add("にお", -1615);
            BW2__.Add("にし", 2748);
            BW2__.Add("にな", 2454);
            BW2__.Add("によ", -7236);
            BW2__.Add("に対", -14943);
            BW2__.Add("に従", -4688);
            BW2__.Add("に関", -11388);
            BW2__.Add("のか", 2093);
            BW2__.Add("ので", -7059);
            BW2__.Add("のに", -6041);
            BW2__.Add("のの", -6125);
            BW2__.Add("はい", 1073);
            BW2__.Add("はが", -1033);
            BW2__.Add("はず", -2532);
            BW2__.Add("ばれ", 1813);
            BW2__.Add("まし", -1316);
            BW2__.Add("まで", -6621);
            BW2__.Add("まれ", 5409);
            BW2__.Add("めて", -3153);
            BW2__.Add("もい", 2230);
            BW2__.Add("もの", -10713);
            BW2__.Add("らか", -944);
            BW2__.Add("らし", -1611);
            BW2__.Add("らに", -1897);
            BW2__.Add("りし", 651);
            BW2__.Add("りま", 1620);
            BW2__.Add("れた", 4270);
            BW2__.Add("れて", 849);
            BW2__.Add("れば", 4114);
            BW2__.Add("ろう", 6067);
            BW2__.Add("われ", 7901);
            BW2__.Add("を通", -11877);
            BW2__.Add("んだ", 728);
            BW2__.Add("んな", -4115);
            BW2__.Add("一人", 602);
            BW2__.Add("一方", -1375);
            BW2__.Add("一日", 970);
            BW2__.Add("一部", -1051);
            BW2__.Add("上が", -4479);
            BW2__.Add("会社", -1116);
            BW2__.Add("出て", 2163);
            BW2__.Add("分の", -7758);
            BW2__.Add("同党", 970);
            BW2__.Add("同日", -913);
            BW2__.Add("大阪", -2471);
            BW2__.Add("委員", -1250);
            BW2__.Add("少な", -1050);
            BW2__.Add("年度", -8669);
            BW2__.Add("年間", -1626);
            BW2__.Add("府県", -2363);
            BW2__.Add("手権", -1982);
            BW2__.Add("新聞", -4066);
            BW2__.Add("日新", -722);
            BW2__.Add("日本", -7068);
            BW2__.Add("日米", 3372);
            BW2__.Add("曜日", -601);
            BW2__.Add("朝鮮", -2355);
            BW2__.Add("本人", -2697);
            BW2__.Add("東京", -1543);
            BW2__.Add("然と", -1384);
            BW2__.Add("社会", -1276);
            BW2__.Add("立て", -990);
            BW2__.Add("第に", -1612);
            BW2__.Add("米国", -4268);
            BW2__.Add("１１", -669);

            BW3__.Add("あた", -2194);
            BW3__.Add("あり", 719);
            BW3__.Add("ある", 3846);
            BW3__.Add("い.", -1185);
            BW3__.Add("い。", -1185);
            BW3__.Add("いい", 5308);
            BW3__.Add("いえ", 2079);
            BW3__.Add("いく", 3029);
            BW3__.Add("いた", 2056);
            BW3__.Add("いっ", 1883);
            BW3__.Add("いる", 5600);
            BW3__.Add("いわ", 1527);
            BW3__.Add("うち", 1117);
            BW3__.Add("うと", 4798);
            BW3__.Add("えと", 1454);
            BW3__.Add("か.", 2857);
            BW3__.Add("か。", 2857);
            BW3__.Add("かけ", -743);
            BW3__.Add("かっ", -4098);
            BW3__.Add("かに", -669);
            BW3__.Add("から", 6520);
            BW3__.Add("かり", -2670);
            BW3__.Add("が,", 1816);
            BW3__.Add("が、", 1816);
            BW3__.Add("がき", -4855);
            BW3__.Add("がけ", -1127);
            BW3__.Add("がっ", -913);
            BW3__.Add("がら", -4977);
            BW3__.Add("がり", -2064);
            BW3__.Add("きた", 1645);
            BW3__.Add("けど", 1374);
            BW3__.Add("こと", 7397);
            BW3__.Add("この", 1542);
            BW3__.Add("ころ", -2757);
            BW3__.Add("さい", -714);
            BW3__.Add("さを", 976);
            BW3__.Add("し,", 1557);
            BW3__.Add("し、", 1557);
            BW3__.Add("しい", -3714);
            BW3__.Add("した", 3562);
            BW3__.Add("して", 1449);
            BW3__.Add("しな", 2608);
            BW3__.Add("しま", 1200);
            BW3__.Add("す.", -1310);
            BW3__.Add("す。", -1310);
            BW3__.Add("する", 6521);
            BW3__.Add("ず,", 3426);
            BW3__.Add("ず、", 3426);
            BW3__.Add("ずに", 841);
            BW3__.Add("そう", 428);
            BW3__.Add("た.", 8875);
            BW3__.Add("た。", 8875);
            BW3__.Add("たい", -594);
            BW3__.Add("たの", 812);
            BW3__.Add("たり", -1183);
            BW3__.Add("たる", -853);
            BW3__.Add("だ.", 4098);
            BW3__.Add("だ。", 4098);
            BW3__.Add("だっ", 1004);
            BW3__.Add("った", -4748);
            BW3__.Add("って", 300);
            BW3__.Add("てい", 6240);
            BW3__.Add("てお", 855);
            BW3__.Add("ても", 302);
            BW3__.Add("です", 1437);
            BW3__.Add("でに", -1482);
            BW3__.Add("では", 2295);
            BW3__.Add("とう", -1387);
            BW3__.Add("とし", 2266);
            BW3__.Add("との", 541);
            BW3__.Add("とも", -3543);
            BW3__.Add("どう", 4664);
            BW3__.Add("ない", 1796);
            BW3__.Add("なく", -903);
            BW3__.Add("など", 2135);
            BW3__.Add("に,", -1021);
            BW3__.Add("に、", -1021);
            BW3__.Add("にし", 1771);
            BW3__.Add("にな", 1906);
            BW3__.Add("には", 2644);
            BW3__.Add("の,", -724);
            BW3__.Add("の、", -724);
            BW3__.Add("の子", -1000);
            BW3__.Add("は,", 1337);
            BW3__.Add("は、", 1337);
            BW3__.Add("べき", 2181);
            BW3__.Add("まし", 1113);
            BW3__.Add("ます", 6943);
            BW3__.Add("まっ", -1549);
            BW3__.Add("まで", 6154);
            BW3__.Add("まれ", -793);
            BW3__.Add("らし", 1479);
            BW3__.Add("られ", 6820);
            BW3__.Add("るる", 3818);
            BW3__.Add("れ,", 854);
            BW3__.Add("れ、", 854);
            BW3__.Add("れた", 1850);
            BW3__.Add("れて", 1375);
            BW3__.Add("れば", -3246);
            BW3__.Add("れる", 1091);
            BW3__.Add("われ", -605);
            BW3__.Add("んだ", 606);
            BW3__.Add("んで", 798);
            BW3__.Add("カ月", 990);
            BW3__.Add("会議", 860);
            BW3__.Add("入り", 1232);
            BW3__.Add("大会", 2217);
            BW3__.Add("始め", 1681);
            BW3__.Add("市", 965);
            BW3__.Add("新聞", -5055);
            BW3__.Add("日,", 974);
            BW3__.Add("日、", 974);
            BW3__.Add("社会", 2024);
            BW3__.Add("ｶ月", 990);

            TC1__.Add("AAA", 1093);
            TC1__.Add("HHH", 1029);
            TC1__.Add("HHM", 580);
            TC1__.Add("HII", 998);
            TC1__.Add("HOH", -390);
            TC1__.Add("HOM", -331);
            TC1__.Add("IHI", 1169);
            TC1__.Add("IOH", -142);
            TC1__.Add("IOI", -1015);
            TC1__.Add("IOM", 467);
            TC1__.Add("MMH", 187);
            TC1__.Add("OOI", -1832);

            TC2__.Add("HHO", 2088);
            TC2__.Add("HII", -1023);
            TC2__.Add("HMM", -1154);
            TC2__.Add("IHI", -1965);
            TC2__.Add("KKH", 703);
            TC2__.Add("OII", -2649);

            TC3__.Add("AAA", -294);
            TC3__.Add("HHH", 346);
            TC3__.Add("HHI", -341);
            TC3__.Add("HII", -1088);
            TC3__.Add("HIK", 731);
            TC3__.Add("HOH", -1486);
            TC3__.Add("IHH", 128);
            TC3__.Add("IHI", -3041);
            TC3__.Add("IHO", -1935);
            TC3__.Add("IIH", -825);
            TC3__.Add("IIM", -1035);
            TC3__.Add("IOI", -542);
            TC3__.Add("KHH", -1216);
            TC3__.Add("KKA", 491);
            TC3__.Add("KKH", -1217);
            TC3__.Add("KOK", -1009);
            TC3__.Add("MHH", -2694);
            TC3__.Add("MHM", -457);
            TC3__.Add("MHO", 123);
            TC3__.Add("MMH", -471);
            TC3__.Add("NNH", -1689);
            TC3__.Add("NNO", 662);
            TC3__.Add("OHO", -3393);

            TC4__.Add("HHH", -203);
            TC4__.Add("HHI", 1344);
            TC4__.Add("HHK", 365);
            TC4__.Add("HHM", -122);
            TC4__.Add("HHN", 182);
            TC4__.Add("HHO", 669);
            TC4__.Add("HIH", 804);
            TC4__.Add("HII", 679);
            TC4__.Add("HOH", 446);
            TC4__.Add("IHH", 695);
            TC4__.Add("IHO", -2324);
            TC4__.Add("IIH", 321);
            TC4__.Add("III", 1497);
            TC4__.Add("IIO", 656);
            TC4__.Add("IOO", 54);
            TC4__.Add("KAK", 4845);
            TC4__.Add("KKA", 3386);
            TC4__.Add("KKK", 3065);
            TC4__.Add("MHH", -405);
            TC4__.Add("MHI", 201);
            TC4__.Add("MMH", -241);
            TC4__.Add("MMM", 661);
            TC4__.Add("MOM", 841);

            TQ1__.Add("BHHH", -227);
            TQ1__.Add("BHHI", 316);
            TQ1__.Add("BHIH", -132);
            TQ1__.Add("BIHH", 60);
            TQ1__.Add("BIII", 1595);
            TQ1__.Add("BNHH", -744);
            TQ1__.Add("BOHH", 225);
            TQ1__.Add("BOOO", -908);
            TQ1__.Add("OAKK", 482);
            TQ1__.Add("OHHH", 281);
            TQ1__.Add("OHIH", 249);
            TQ1__.Add("OIHI", 200);
            TQ1__.Add("OIIH", -68);

            TQ2__.Add("BIHH", -1401);
            TQ2__.Add("BIII", -1033);
            TQ2__.Add("BKAK", -543);
            TQ2__.Add("BOOO", -5591);

            TQ3__.Add("BHHH", 478);
            TQ3__.Add("BHHM", -1073);
            TQ3__.Add("BHIH", 222);
            TQ3__.Add("BHII", -504);
            TQ3__.Add("BIIH", -116);
            TQ3__.Add("BIII", -105);
            TQ3__.Add("BMHI", -863);
            TQ3__.Add("BMHM", -464);
            TQ3__.Add("BOMH", 620);
            TQ3__.Add("OHHH", 346);
            TQ3__.Add("OHHI", 1729);
            TQ3__.Add("OHII", 997);
            TQ3__.Add("OHMH", 481);
            TQ3__.Add("OIHH", 623);
            TQ3__.Add("OIIH", 1344);
            TQ3__.Add("OKAK", 2792);
            TQ3__.Add("OKHH", 587);
            TQ3__.Add("OKKA", 679);
            TQ3__.Add("OOHH", 110);
            TQ3__.Add("OOII", -685);

            TQ4__.Add("BHHH", -721);
            TQ4__.Add("BHHM", -3604);
            TQ4__.Add("BHII", -966);
            TQ4__.Add("BIIH", -607);
            TQ4__.Add("BIII", -2181);
            TQ4__.Add("OAAA", -2763);
            TQ4__.Add("OAKK", 180);
            TQ4__.Add("OHHH", -294);
            TQ4__.Add("OHHI", 2446);
            TQ4__.Add("OHHO", 480);
            TQ4__.Add("OHIH", -1573);
            TQ4__.Add("OIHH", 1935);
            TQ4__.Add("OIHI", -493);
            TQ4__.Add("OIIH", 626);
            TQ4__.Add("OIII", -4007);
            TQ4__.Add("OKAK", -8156);

            TW1__.Add("につい", -4681);
            TW1__.Add("東京都", 2026);

            TW2__.Add("ある程", -2049);
            TW2__.Add("いった", -1256);
            TW2__.Add("ころが", -2434);
            TW2__.Add("しょう", 3873);
            TW2__.Add("その後", -4430);
            TW2__.Add("だって", -1049);
            TW2__.Add("ていた", 1833);
            TW2__.Add("として", -4657);
            TW2__.Add("ともに", -4517);
            TW2__.Add("もので", 1882);
            TW2__.Add("一気に", -792);
            TW2__.Add("初めて", -1512);
            TW2__.Add("同時に", -8097);
            TW2__.Add("大きな", -1255);
            TW2__.Add("対して", -2721);
            TW2__.Add("社会党", -3216);

            TW3__.Add("いただ", -1734);
            TW3__.Add("してい", 1314);
            TW3__.Add("として", -4314);
            TW3__.Add("につい", -5483);
            TW3__.Add("にとっ", -5989);
            TW3__.Add("に当た", -6247);
            TW3__.Add("ので,", -727);
            TW3__.Add("ので、", -727);
            TW3__.Add("のもの", -600);
            TW3__.Add("れから", -3752);
            TW3__.Add("十二月", -2287);

            TW4__.Add("いう.", 8576);
            TW4__.Add("いう。", 8576);
            TW4__.Add("からな", -2348);
            TW4__.Add("してい", 2958);
            TW4__.Add("たが,", 1516);
            TW4__.Add("たが、", 1516);
            TW4__.Add("ている", 1538);
            TW4__.Add("という", 1349);
            TW4__.Add("ました", 5543);
            TW4__.Add("ません", 1097);
            TW4__.Add("ようと", -4258);
            TW4__.Add("よると", 5865);

            UC1__.Add("A", 484);
            UC1__.Add("K", 93);
            UC1__.Add("M", 645);
            UC1__.Add("O", -505);

            UC2__.Add("A", 819);
            UC2__.Add("H", 1059);
            UC2__.Add("I", 409);
            UC2__.Add("M", 3987);
            UC2__.Add("N", 5775);
            UC2__.Add("O", 646);

            UC3__.Add("A", -1370);
            UC3__.Add("I", 2311);

            UC4__.Add("A", -2643);
            UC4__.Add("H", 1809);
            UC4__.Add("I", -1032);
            UC4__.Add("K", -3450);
            UC4__.Add("M", 3565);
            UC4__.Add("N", 3876);
            UC4__.Add("O", 6646);

            UC5__.Add("H", 313);
            UC5__.Add("I", -1238);
            UC5__.Add("K", -799);
            UC5__.Add("M", 539);
            UC5__.Add("O", -831);

            UC6__.Add("H", -506);
            UC6__.Add("I", -253);
            UC6__.Add("K", 87);
            UC6__.Add("M", 247);
            UC6__.Add("O", -387);

            UP1__.Add("O", -214);

            UP2__.Add("B", 69);
            UP2__.Add("O", 935);

            UP3__.Add("B", 189);

            UQ1__.Add("BH", 21);
            UQ1__.Add("BI", -12);
            UQ1__.Add("BK", -99);
            UQ1__.Add("BN", 142);
            UQ1__.Add("BO", -56);
            UQ1__.Add("OH", -95);
            UQ1__.Add("OI", 477);
            UQ1__.Add("OK", 410);
            UQ1__.Add("OO", -2422);

            UQ2__.Add("BH", 216);
            UQ2__.Add("BI", 113);
            UQ2__.Add("OK", 1759);

            UQ3__.Add("BA", -479);
            UQ3__.Add("BH", 42);
            UQ3__.Add("BI", 1913);
            UQ3__.Add("BK", -7198);
            UQ3__.Add("BM", 3160);
            UQ3__.Add("BN", 6427);
            UQ3__.Add("BO", 14761);
            UQ3__.Add("OI", -827);
            UQ3__.Add("ON", -3212);

            UW1__.Add(",", 156);
            UW1__.Add("、", 156);
            UW1__.Add("「", -463);
            UW1__.Add("あ", -941);
            UW1__.Add("う", -127);
            UW1__.Add("が", -553);
            UW1__.Add("き", 121);
            UW1__.Add("こ", 505);
            UW1__.Add("で", -201);
            UW1__.Add("と", -547);
            UW1__.Add("ど", -123);
            UW1__.Add("に", -789);
            UW1__.Add("の", -185);
            UW1__.Add("は", -847);
            UW1__.Add("も", -466);
            UW1__.Add("や", -470);
            UW1__.Add("よ", 182);
            UW1__.Add("ら", -292);
            UW1__.Add("り", 208);
            UW1__.Add("れ", 169);
            UW1__.Add("を", -446);
            UW1__.Add("ん", -137);
            UW1__.Add("・", -135);
            UW1__.Add("主", -402);
            UW1__.Add("京", -268);
            UW1__.Add("区", -912);
            UW1__.Add("午", 871);
            UW1__.Add("国", -460);
            UW1__.Add("大", 561);
            UW1__.Add("委", 729);
            UW1__.Add("市", -411);
            UW1__.Add("日", -141);
            UW1__.Add("理", 361);
            UW1__.Add("生", -408);
            UW1__.Add("県", -386);
            UW1__.Add("都", -718);
            UW1__.Add("｢", -463);
            UW1__.Add("･", -135);

            UW2__.Add(",", -829);
            UW2__.Add("、", -829);
            UW2__.Add("〇", 892);
            UW2__.Add("「", -645);
            UW2__.Add("」", 3145);
            UW2__.Add("あ", -538);
            UW2__.Add("い", 505);
            UW2__.Add("う", 134);
            UW2__.Add("お", -502);
            UW2__.Add("か", 1454);
            UW2__.Add("が", -856);
            UW2__.Add("く", -412);
            UW2__.Add("こ", 1141);
            UW2__.Add("さ", 878);
            UW2__.Add("ざ", 540);
            UW2__.Add("し", 1529);
            UW2__.Add("す", -675);
            UW2__.Add("せ", 300);
            UW2__.Add("そ", -1011);
            UW2__.Add("た", 188);
            UW2__.Add("だ", 1837);
            UW2__.Add("つ", -949);
            UW2__.Add("て", -291);
            UW2__.Add("で", -268);
            UW2__.Add("と", -981);
            UW2__.Add("ど", 1273);
            UW2__.Add("な", 1063);
            UW2__.Add("に", -1764);
            UW2__.Add("の", 130);
            UW2__.Add("は", -409);
            UW2__.Add("ひ", -1273);
            UW2__.Add("べ", 1261);
            UW2__.Add("ま", 600);
            UW2__.Add("も", -1263);
            UW2__.Add("や", -402);
            UW2__.Add("よ", 1639);
            UW2__.Add("り", -579);
            UW2__.Add("る", -694);
            UW2__.Add("れ", 571);
            UW2__.Add("を", -2516);
            UW2__.Add("ん", 2095);
            UW2__.Add("ア", -587);
            UW2__.Add("カ", 306);
            UW2__.Add("キ", 568);
            UW2__.Add("ッ", 831);
            UW2__.Add("三", -758);
            UW2__.Add("不", -2150);
            UW2__.Add("世", -302);
            UW2__.Add("中", -968);
            UW2__.Add("主", -861);
            UW2__.Add("事", 492);
            UW2__.Add("人", -123);
            UW2__.Add("会", 978);
            UW2__.Add("保", 362);
            UW2__.Add("入", 548);
            UW2__.Add("初", -3025);
            UW2__.Add("副", -1566);
            UW2__.Add("北", -3414);
            UW2__.Add("区", -422);
            UW2__.Add("大", -1769);
            UW2__.Add("天", -865);
            UW2__.Add("太", -483);
            UW2__.Add("子", -1519);
            UW2__.Add("学", 760);
            UW2__.Add("実", 1023);
            UW2__.Add("小", -2009);
            UW2__.Add("市", -813);
            UW2__.Add("年", -1060);
            UW2__.Add("強", 1067);
            UW2__.Add("手", -1519);
            UW2__.Add("揺", -1033);
            UW2__.Add("政", 1522);
            UW2__.Add("文", -1355);
            UW2__.Add("新", -1682);
            UW2__.Add("日", -1815);
            UW2__.Add("明", -1462);
            UW2__.Add("最", -630);
            UW2__.Add("朝", -1843);
            UW2__.Add("本", -1650);
            UW2__.Add("東", -931);
            UW2__.Add("果", -665);
            UW2__.Add("次", -2378);
            UW2__.Add("民", -180);
            UW2__.Add("気", -1740);
            UW2__.Add("理", 752);
            UW2__.Add("発", 529);
            UW2__.Add("目", -1584);
            UW2__.Add("相", -242);
            UW2__.Add("県", -1165);
            UW2__.Add("立", -763);
            UW2__.Add("第", 810);
            UW2__.Add("米", 509);
            UW2__.Add("自", -1353);
            UW2__.Add("行", 838);
            UW2__.Add("西", -744);
            UW2__.Add("見", -3874);
            UW2__.Add("調", 1010);
            UW2__.Add("議", 1198);
            UW2__.Add("込", 3041);
            UW2__.Add("開", 1758);
            UW2__.Add("間", -1257);
            UW2__.Add("｢", -645);
            UW2__.Add("｣", 3145);
            UW2__.Add("ｯ", 831);
            UW2__.Add("ｱ", -587);
            UW2__.Add("ｶ", 306);
            UW2__.Add("ｷ", 568);

            UW3__.Add(",", 4889);
            UW3__.Add("1", -800);
            UW3__.Add("−", -1723);
            UW3__.Add("、", 4889);
            UW3__.Add("々", -2311);
            UW3__.Add("〇", 5827);
            UW3__.Add("」", 2670);
            UW3__.Add("〓", -3573);
            UW3__.Add("あ", -2696);
            UW3__.Add("い", 1006);
            UW3__.Add("う", 2342);
            UW3__.Add("え", 1983);
            UW3__.Add("お", -4864);
            UW3__.Add("か", -1163);
            UW3__.Add("が", 3271);
            UW3__.Add("く", 1004);
            UW3__.Add("け", 388);
            UW3__.Add("げ", 401);
            UW3__.Add("こ", -3552);
            UW3__.Add("ご", -3116);
            UW3__.Add("さ", -1058);
            UW3__.Add("し", -395);
            UW3__.Add("す", 584);
            UW3__.Add("せ", 3685);
            UW3__.Add("そ", -5228);
            UW3__.Add("た", 842);
            UW3__.Add("ち", -521);
            UW3__.Add("っ", -1444);
            UW3__.Add("つ", -1081);
            UW3__.Add("て", 6167);
            UW3__.Add("で", 2318);
            UW3__.Add("と", 1691);
            UW3__.Add("ど", -899);
            UW3__.Add("な", -2788);
            UW3__.Add("に", 2745);
            UW3__.Add("の", 4056);
            UW3__.Add("は", 4555);
            UW3__.Add("ひ", -2171);
            UW3__.Add("ふ", -1798);
            UW3__.Add("へ", 1199);
            UW3__.Add("ほ", -5516);
            UW3__.Add("ま", -4384);
            UW3__.Add("み", -120);
            UW3__.Add("め", 1205);
            UW3__.Add("も", 2323);
            UW3__.Add("や", -788);
            UW3__.Add("よ", -202);
            UW3__.Add("ら", 727);
            UW3__.Add("り", 649);
            UW3__.Add("る", 5905);
            UW3__.Add("れ", 2773);
            UW3__.Add("わ", -1207);
            UW3__.Add("を", 6620);
            UW3__.Add("ん", -518);
            UW3__.Add("ア", 551);
            UW3__.Add("グ", 1319);
            UW3__.Add("ス", 874);
            UW3__.Add("ッ", -1350);
            UW3__.Add("ト", 521);
            UW3__.Add("ム", 1109);
            UW3__.Add("ル", 1591);
            UW3__.Add("ロ", 2201);
            UW3__.Add("ン", 278);
            UW3__.Add("・", -3794);
            UW3__.Add("一", -1619);
            UW3__.Add("下", -1759);
            UW3__.Add("世", -2087);
            UW3__.Add("両", 3815);
            UW3__.Add("中", 653);
            UW3__.Add("主", -758);
            UW3__.Add("予", -1193);
            UW3__.Add("二", 974);
            UW3__.Add("人", 2742);
            UW3__.Add("今", 792);
            UW3__.Add("他", 1889);
            UW3__.Add("以", -1368);
            UW3__.Add("低", 811);
            UW3__.Add("何", 4265);
            UW3__.Add("作", -361);
            UW3__.Add("保", -2439);
            UW3__.Add("元", 4858);
            UW3__.Add("党", 3593);
            UW3__.Add("全", 1574);
            UW3__.Add("公", -3030);
            UW3__.Add("六", 755);
            UW3__.Add("共", -1880);
            UW3__.Add("円", 5807);
            UW3__.Add("再", 3095);
            UW3__.Add("分", 457);
            UW3__.Add("初", 2475);
            UW3__.Add("別", 1129);
            UW3__.Add("前", 2286);
            UW3__.Add("副", 4437);
            UW3__.Add("力", 365);
            UW3__.Add("動", -949);
            UW3__.Add("務", -1872);
            UW3__.Add("化", 1327);
            UW3__.Add("北", -1038);
            UW3__.Add("区", 4646);
            UW3__.Add("千", -2309);
            UW3__.Add("午", -783);
            UW3__.Add("協", -1006);
            UW3__.Add("口", 483);
            UW3__.Add("右", 1233);
            UW3__.Add("各", 3588);
            UW3__.Add("合", -241);
            UW3__.Add("同", 3906);
            UW3__.Add("和", -837);
            UW3__.Add("員", 4513);
            UW3__.Add("国", 642);
            UW3__.Add("型", 1389);
            UW3__.Add("場", 1219);
            UW3__.Add("外", -241);
            UW3__.Add("妻", 2016);
            UW3__.Add("学", -1356);
            UW3__.Add("安", -423);
            UW3__.Add("実", -1008);
            UW3__.Add("家", 1078);
            UW3__.Add("小", -513);
            UW3__.Add("少", -3102);
            UW3__.Add("州", 1155);
            UW3__.Add("市", 3197);
            UW3__.Add("平", -1804);
            UW3__.Add("年", 2416);
            UW3__.Add("広", -1030);
            UW3__.Add("府", 1605);
            UW3__.Add("度", 1452);
            UW3__.Add("建", -2352);
            UW3__.Add("当", -3885);
            UW3__.Add("得", 1905);
            UW3__.Add("思", -1291);
            UW3__.Add("性", 1822);
            UW3__.Add("戸", -488);
            UW3__.Add("指", -3973);
            UW3__.Add("政", -2013);
            UW3__.Add("教", -1479);
            UW3__.Add("数", 3222);
            UW3__.Add("文", -1489);
            UW3__.Add("新", 1764);
            UW3__.Add("日", 2099);
            UW3__.Add("旧", 5792);
            UW3__.Add("昨", -661);
            UW3__.Add("時", -1248);
            UW3__.Add("曜", -951);
            UW3__.Add("最", -937);
            UW3__.Add("月", 4125);
            UW3__.Add("期", 360);
            UW3__.Add("李", 3094);
            UW3__.Add("村", 364);
            UW3__.Add("東", -805);
            UW3__.Add("核", 5156);
            UW3__.Add("森", 2438);
            UW3__.Add("業", 484);
            UW3__.Add("氏", 2613);
            UW3__.Add("民", -1694);
            UW3__.Add("決", -1073);
            UW3__.Add("法", 1868);
            UW3__.Add("海", -495);
            UW3__.Add("無", 979);
            UW3__.Add("物", 461);
            UW3__.Add("特", -3850);
            UW3__.Add("生", -273);
            UW3__.Add("用", 914);
            UW3__.Add("町", 1215);
            UW3__.Add("的", 7313);
            UW3__.Add("直", -1835);
            UW3__.Add("省", 792);
            UW3__.Add("県", 6293);
            UW3__.Add("知", -1528);
            UW3__.Add("私", 4231);
            UW3__.Add("税", 401);
            UW3__.Add("立", -960);
            UW3__.Add("第", 1201);
            UW3__.Add("米", 7767);
            UW3__.Add("系", 3066);
            UW3__.Add("約", 3663);
            UW3__.Add("級", 1384);
            UW3__.Add("統", -4229);
            UW3__.Add("総", 1163);
            UW3__.Add("線", 1255);
            UW3__.Add("者", 6457);
            UW3__.Add("能", 725);
            UW3__.Add("自", -2869);
            UW3__.Add("英", 785);
            UW3__.Add("見", 1044);
            UW3__.Add("調", -562);
            UW3__.Add("財", -733);
            UW3__.Add("費", 1777);
            UW3__.Add("車", 1835);
            UW3__.Add("軍", 1375);
            UW3__.Add("込", -1504);
            UW3__.Add("通", -1136);
            UW3__.Add("選", -681);
            UW3__.Add("郎", 1026);
            UW3__.Add("郡", 4404);
            UW3__.Add("部", 1200);
            UW3__.Add("金", 2163);
            UW3__.Add("長", 421);
            UW3__.Add("開", -1432);
            UW3__.Add("間", 1302);
            UW3__.Add("関", -1282);
            UW3__.Add("雨", 2009);
            UW3__.Add("電", -1045);
            UW3__.Add("非", 2066);
            UW3__.Add("駅", 1620);
            UW3__.Add("１", -800);
            UW3__.Add("｣", 2670);
            UW3__.Add("･", -3794);
            UW3__.Add("ｯ", -1350);
            UW3__.Add("ｱ", 551);
            UW3__.Add("ｸﾞ", 1319);
            UW3__.Add("ｽ", 874);
            UW3__.Add("ﾄ", 521);
            UW3__.Add("ﾑ", 1109);
            UW3__.Add("ﾙ", 1591);
            UW3__.Add("ﾛ", 2201);
            UW3__.Add("ﾝ", 278);

            UW4__.Add(",", 3930);
            UW4__.Add(".", 3508);
            UW4__.Add("―", -4841);
            UW4__.Add("、", 3930);
            UW4__.Add("。", 3508);
            UW4__.Add("〇", 4999);
            UW4__.Add("「", 1895);
            UW4__.Add("」", 3798);
            UW4__.Add("〓", -5156);
            UW4__.Add("あ", 4752);
            UW4__.Add("い", -3435);
            UW4__.Add("う", -640);
            UW4__.Add("え", -2514);
            UW4__.Add("お", 2405);
            UW4__.Add("か", 530);
            UW4__.Add("が", 6006);
            UW4__.Add("き", -4482);
            UW4__.Add("ぎ", -3821);
            UW4__.Add("く", -3788);
            UW4__.Add("け", -4376);
            UW4__.Add("げ", -4734);
            UW4__.Add("こ", 2255);
            UW4__.Add("ご", 1979);
            UW4__.Add("さ", 2864);
            UW4__.Add("し", -843);
            UW4__.Add("じ", -2506);
            UW4__.Add("す", -731);
            UW4__.Add("ず", 1251);
            UW4__.Add("せ", 181);
            UW4__.Add("そ", 4091);
            UW4__.Add("た", 5034);
            UW4__.Add("だ", 5408);
            UW4__.Add("ち", -3654);
            UW4__.Add("っ", -5882);
            UW4__.Add("つ", -1659);
            UW4__.Add("て", 3994);
            UW4__.Add("で", 7410);
            UW4__.Add("と", 4547);
            UW4__.Add("な", 5433);
            UW4__.Add("に", 6499);
            UW4__.Add("ぬ", 1853);
            UW4__.Add("ね", 1413);
            UW4__.Add("の", 7396);
            UW4__.Add("は", 8578);
            UW4__.Add("ば", 1940);
            UW4__.Add("ひ", 4249);
            UW4__.Add("び", -4134);
            UW4__.Add("ふ", 1345);
            UW4__.Add("へ", 6665);
            UW4__.Add("べ", -744);
            UW4__.Add("ほ", 1464);
            UW4__.Add("ま", 1051);
            UW4__.Add("み", -2082);
            UW4__.Add("む", -882);
            UW4__.Add("め", -5046);
            UW4__.Add("も", 4169);
            UW4__.Add("ゃ", -2666);
            UW4__.Add("や", 2795);
            UW4__.Add("ょ", -1544);
            UW4__.Add("よ", 3351);
            UW4__.Add("ら", -2922);
            UW4__.Add("り", -9726);
            UW4__.Add("る", -14896);
            UW4__.Add("れ", -2613);
            UW4__.Add("ろ", -4570);
            UW4__.Add("わ", -1783);
            UW4__.Add("を", 13150);
            UW4__.Add("ん", -2352);
            UW4__.Add("カ", 2145);
            UW4__.Add("コ", 1789);
            UW4__.Add("セ", 1287);
            UW4__.Add("ッ", -724);
            UW4__.Add("ト", -403);
            UW4__.Add("メ", -1635);
            UW4__.Add("ラ", -881);
            UW4__.Add("リ", -541);
            UW4__.Add("ル", -856);
            UW4__.Add("ン", -3637);
            UW4__.Add("・", -4371);
            UW4__.Add("ー", -11870);
            UW4__.Add("一", -2069);
            UW4__.Add("中", 2210);
            UW4__.Add("予", 782);
            UW4__.Add("事", -190);
            UW4__.Add("井", -1768);
            UW4__.Add("人", 1036);
            UW4__.Add("以", 544);
            UW4__.Add("会", 950);
            UW4__.Add("体", -1286);
            UW4__.Add("作", 530);
            UW4__.Add("側", 4292);
            UW4__.Add("先", 601);
            UW4__.Add("党", -2006);
            UW4__.Add("共", -1212);
            UW4__.Add("内", 584);
            UW4__.Add("円", 788);
            UW4__.Add("初", 1347);
            UW4__.Add("前", 1623);
            UW4__.Add("副", 3879);
            UW4__.Add("力", -302);
            UW4__.Add("動", -740);
            UW4__.Add("務", -2715);
            UW4__.Add("化", 776);
            UW4__.Add("区", 4517);
            UW4__.Add("協", 1013);
            UW4__.Add("参", 1555);
            UW4__.Add("合", -1834);
            UW4__.Add("和", -681);
            UW4__.Add("員", -910);
            UW4__.Add("器", -851);
            UW4__.Add("回", 1500);
            UW4__.Add("国", -619);
            UW4__.Add("園", -1200);
            UW4__.Add("地", 866);
            UW4__.Add("場", -1410);
            UW4__.Add("塁", -2094);
            UW4__.Add("士", -1413);
            UW4__.Add("多", 1067);
            UW4__.Add("大", 571);
            UW4__.Add("子", -4802);
            UW4__.Add("学", -1397);
            UW4__.Add("定", -1057);
            UW4__.Add("寺", -809);
            UW4__.Add("小", 1910);
            UW4__.Add("屋", -1328);
            UW4__.Add("山", -1500);
            UW4__.Add("島", -2056);
            UW4__.Add("川", -2667);
            UW4__.Add("市", 2771);
            UW4__.Add("年", 374);
            UW4__.Add("庁", -4556);
            UW4__.Add("後", 456);
            UW4__.Add("性", 553);
            UW4__.Add("感", 916);
            UW4__.Add("所", -1566);
            UW4__.Add("支", 856);
            UW4__.Add("改", 787);
            UW4__.Add("政", 2182);
            UW4__.Add("教", 704);
            UW4__.Add("文", 522);
            UW4__.Add("方", -856);
            UW4__.Add("日", 1798);
            UW4__.Add("時", 1829);
            UW4__.Add("最", 845);
            UW4__.Add("月", -9066);
            UW4__.Add("木", -485);
            UW4__.Add("来", -442);
            UW4__.Add("校", -360);
            UW4__.Add("業", -1043);
            UW4__.Add("氏", 5388);
            UW4__.Add("民", -2716);
            UW4__.Add("気", -910);
            UW4__.Add("沢", -939);
            UW4__.Add("済", -543);
            UW4__.Add("物", -735);
            UW4__.Add("率", 672);
            UW4__.Add("球", -1267);
            UW4__.Add("生", -1286);
            UW4__.Add("産", -1101);
            UW4__.Add("田", -2900);
            UW4__.Add("町", 1826);
            UW4__.Add("的", 2586);
            UW4__.Add("目", 922);
            UW4__.Add("省", -3485);
            UW4__.Add("県", 2997);
            UW4__.Add("空", -867);
            UW4__.Add("立", -2112);
            UW4__.Add("第", 788);
            UW4__.Add("米", 2937);
            UW4__.Add("系", 786);
            UW4__.Add("約", 2171);
            UW4__.Add("経", 1146);
            UW4__.Add("統", -1169);
            UW4__.Add("総", 940);
            UW4__.Add("線", -994);
            UW4__.Add("署", 749);
            UW4__.Add("者", 2145);
            UW4__.Add("能", -730);
            UW4__.Add("般", -852);
            UW4__.Add("行", -792);
            UW4__.Add("規", 792);
            UW4__.Add("警", -1184);
            UW4__.Add("議", -244);
            UW4__.Add("谷", -1000);
            UW4__.Add("賞", 730);
            UW4__.Add("車", -1481);
            UW4__.Add("軍", 1158);
            UW4__.Add("輪", -1433);
            UW4__.Add("込", -3370);
            UW4__.Add("近", 929);
            UW4__.Add("道", -1291);
            UW4__.Add("選", 2596);
            UW4__.Add("郎", -4866);
            UW4__.Add("都", 1192);
            UW4__.Add("野", -1100);
            UW4__.Add("銀", -2213);
            UW4__.Add("長", 357);
            UW4__.Add("間", -2344);
            UW4__.Add("院", -2297);
            UW4__.Add("際", -2604);
            UW4__.Add("電", -878);
            UW4__.Add("領", -1659);
            UW4__.Add("題", -792);
            UW4__.Add("館", -1984);
            UW4__.Add("首", 1749);
            UW4__.Add("高", 2120);
            UW4__.Add("｢", 1895);
            UW4__.Add("｣", 3798);
            UW4__.Add("･", -4371);
            UW4__.Add("ｯ", -724);
            UW4__.Add("ｰ", -11870);
            UW4__.Add("ｶ", 2145);
            UW4__.Add("ｺ", 1789);
            UW4__.Add("ｾ", 1287);
            UW4__.Add("ﾄ", -403);
            UW4__.Add("ﾒ", -1635);
            UW4__.Add("ﾗ", -881);
            UW4__.Add("ﾘ", -541);
            UW4__.Add("ﾙ", -856);
            UW4__.Add("ﾝ", -3637);

            UW5__.Add(",", 465);
            UW5__.Add(".", -299);
            UW5__.Add("1", -514);
            UW5__.Add("E2", -32768);
            UW5__.Add("]", -2762);
            UW5__.Add("、", 465);
            UW5__.Add("。", -299);
            UW5__.Add("「", 363);
            UW5__.Add("あ", 1655);
            UW5__.Add("い", 331);
            UW5__.Add("う", -503);
            UW5__.Add("え", 1199);
            UW5__.Add("お", 527);
            UW5__.Add("か", 647);
            UW5__.Add("が", -421);
            UW5__.Add("き", 1624);
            UW5__.Add("ぎ", 1971);
            UW5__.Add("く", 312);
            UW5__.Add("げ", -983);
            UW5__.Add("さ", -1537);
            UW5__.Add("し", -1371);
            UW5__.Add("す", -852);
            UW5__.Add("だ", -1186);
            UW5__.Add("ち", 1093);
            UW5__.Add("っ", 52);
            UW5__.Add("つ", 921);
            UW5__.Add("て", -18);
            UW5__.Add("で", -850);
            UW5__.Add("と", -127);
            UW5__.Add("ど", 1682);
            UW5__.Add("な", -787);
            UW5__.Add("に", -1224);
            UW5__.Add("の", -635);
            UW5__.Add("は", -578);
            UW5__.Add("べ", 1001);
            UW5__.Add("み", 502);
            UW5__.Add("め", 865);
            UW5__.Add("ゃ", 3350);
            UW5__.Add("ょ", 854);
            UW5__.Add("り", -208);
            UW5__.Add("る", 429);
            UW5__.Add("れ", 504);
            UW5__.Add("わ", 419);
            UW5__.Add("を", -1264);
            UW5__.Add("ん", 327);
            UW5__.Add("イ", 241);
            UW5__.Add("ル", 451);
            UW5__.Add("ン", -343);
            UW5__.Add("中", -871);
            UW5__.Add("京", 722);
            UW5__.Add("会", -1153);
            UW5__.Add("党", -654);
            UW5__.Add("務", 3519);
            UW5__.Add("区", -901);
            UW5__.Add("告", 848);
            UW5__.Add("員", 2104);
            UW5__.Add("大", -1296);
            UW5__.Add("学", -548);
            UW5__.Add("定", 1785);
            UW5__.Add("嵐", -1304);
            UW5__.Add("市", -2991);
            UW5__.Add("席", 921);
            UW5__.Add("年", 1763);
            UW5__.Add("思", 872);
            UW5__.Add("所", -814);
            UW5__.Add("挙", 1618);
            UW5__.Add("新", -1682);
            UW5__.Add("日", 218);
            UW5__.Add("月", -4353);
            UW5__.Add("査", 932);
            UW5__.Add("格", 1356);
            UW5__.Add("機", -1508);
            UW5__.Add("氏", -1347);
            UW5__.Add("田", 240);
            UW5__.Add("町", -3912);
            UW5__.Add("的", -3149);
            UW5__.Add("相", 1319);
            UW5__.Add("省", -1052);
            UW5__.Add("県", -4003);
            UW5__.Add("研", -997);
            UW5__.Add("社", -278);
            UW5__.Add("空", -813);
            UW5__.Add("統", 1955);
            UW5__.Add("者", -2233);
            UW5__.Add("表", 663);
            UW5__.Add("語", -1073);
            UW5__.Add("議", 1219);
            UW5__.Add("選", -1018);
            UW5__.Add("郎", -368);
            UW5__.Add("長", 786);
            UW5__.Add("間", 1191);
            UW5__.Add("題", 2368);
            UW5__.Add("館", -689);
            UW5__.Add("１", -514);
            UW5__.Add("Ｅ２", -32768);
            UW5__.Add("｢", 363);
            UW5__.Add("ｲ", 241);
            UW5__.Add("ﾙ", 451);
            UW5__.Add("ﾝ", -343);

            UW6__.Add(",", 227);
            UW6__.Add(".", 808);
            UW6__.Add("1", -270);
            UW6__.Add("E1", 306);
            UW6__.Add("、", 227);
            UW6__.Add("。", 808);
            UW6__.Add("あ", -307);
            UW6__.Add("う", 189);
            UW6__.Add("か", 241);
            UW6__.Add("が", -73);
            UW6__.Add("く", -121);
            UW6__.Add("こ", -200);
            UW6__.Add("じ", 1782);
            UW6__.Add("す", 383);
            UW6__.Add("た", -428);
            UW6__.Add("っ", 573);
            UW6__.Add("て", -1014);
            UW6__.Add("で", 101);
            UW6__.Add("と", -105);
            UW6__.Add("な", -253);
            UW6__.Add("に", -149);
            UW6__.Add("の", -417);
            UW6__.Add("は", -236);
            UW6__.Add("も", -206);
            UW6__.Add("り", 187);
            UW6__.Add("る", -135);
            UW6__.Add("を", 195);
            UW6__.Add("ル", -673);
            UW6__.Add("ン", -496);
            UW6__.Add("一", -277);
            UW6__.Add("中", 201);
            UW6__.Add("件", -800);
            UW6__.Add("会", 624);
            UW6__.Add("前", 302);
            UW6__.Add("区", 1792);
            UW6__.Add("員", -1212);
            UW6__.Add("委", 798);
            UW6__.Add("学", -960);
            UW6__.Add("市", 887);
            UW6__.Add("広", -695);
            UW6__.Add("後", 535);
            UW6__.Add("業", -697);
            UW6__.Add("相", 753);
            UW6__.Add("社", -507);
            UW6__.Add("福", 974);
            UW6__.Add("空", -822);
            UW6__.Add("者", 1811);
            UW6__.Add("連", 463);
            UW6__.Add("郎", 1082);
            UW6__.Add("１", -270);
            UW6__.Add("Ｅ１", 306);
            UW6__.Add("ﾙ", -673);
            UW6__.Add("ﾝ", -496);
        }
        #endregion

        #region オリジナルTinySegmenter
        private Dictionary<Regex, string> chartype_;

        public TinySegmenter()
        {
            this.InitializeConstants();

            Dictionary<string, string> patterns = new Dictionary<string, string>();
            patterns.Add("[一二三四五六七八九十百千万億兆]", "M");
            patterns.Add("[一-龠々〆ヵヶ]", "H");
            patterns.Add("[ぁ-ん]", "I");
            patterns.Add("[ァ-ヴーｱ-ﾝﾞｰ]", "K");
            patterns.Add("[a-zA-Zａ-ｚＡ-Ｚ]", "A");
            patterns.Add("[0-9０-９]", "N");

            this.chartype_ = new Dictionary<Regex, string>();
            foreach (string i in patterns.Keys)
            {
                Regex reg = new Regex(i);
                this.chartype_.Add(reg, patterns[i]);
            }
        }

        private string ctype_(string str)
        {
            foreach (KeyValuePair<Regex, string> i in this.chartype_)
            {
                if (i.Key.IsMatch(str))
                {
                    return i.Value;
                }
            }
            return "O";
        }

        private int ts_(Dictionary<string, int> dic, string v)
        {
            int result;
            if (dic.TryGetValue(v, out result))
            {
                return result;
            }
            return 0;
        }

        public string[] Segment(string input)
        {
            if (string.IsNullOrEmpty(input))
                return new string[] { };

            List<string> result = new List<string>();
            List<string> seg = new List<string>();
            seg.Add("B3");
            seg.Add("B2");
            seg.Add("B1");
            List<string> ctype = new List<string>();
            ctype.Add("O");
            ctype.Add("O");
            ctype.Add("O");
            //char[] o = input.ToCharArray();
            for (int i = 0; i < input.Length; i++)
            {
                string c = input[i].ToString();
                seg.Add(c);
                ctype.Add(this.ctype_(c));
            }
            seg.Add("E1");
            seg.Add("E2");
            seg.Add("E3");
            ctype.Add("O");
            ctype.Add("O");
            ctype.Add("O");
            string word = seg[3];
            string p1 = "U";
            string p2 = "U";
            string p3 = "U";
            for (int i = 4; i < seg.Count - 3; i++)
            {
                int score = this.BIAS__;
                string w1 = seg[i - 3];
                string w2 = seg[i - 2];
                string w3 = seg[i - 1];
                string w4 = seg[i];
                string w5 = seg[i + 1];
                string w6 = seg[i + 2];
                string c1 = ctype[i - 3];
                string c2 = ctype[i - 2];
                string c3 = ctype[i - 1];
                string c4 = ctype[i];
                string c5 = ctype[i + 1];
                string c6 = ctype[i + 2];
                score += this.ts_(this.UP1__, p1);
                score += this.ts_(this.UP2__, p2);
                score += this.ts_(this.UP3__, p3);
                score += this.ts_(this.BP1__, p1 + p2);
                score += this.ts_(this.BP2__, p2 + p3);
                score += this.ts_(this.UW1__, w1);
                score += this.ts_(this.UW2__, w2);
                score += this.ts_(this.UW3__, w3);
                score += this.ts_(this.UW4__, w4);
                score += this.ts_(this.UW5__, w5);
                score += this.ts_(this.UW6__, w6);
                score += this.ts_(this.BW1__, w2 + w3);
                score += this.ts_(this.BW2__, w3 + w4);
                score += this.ts_(this.BW3__, w4 + w5);
                score += this.ts_(this.TW1__, w1 + w2 + w3);
                score += this.ts_(this.TW2__, w2 + w3 + w4);
                score += this.ts_(this.TW3__, w3 + w4 + w5);
                score += this.ts_(this.TW4__, w4 + w5 + w6);
                score += this.ts_(this.UC1__, c1);
                score += this.ts_(this.UC2__, c2);
                score += this.ts_(this.UC3__, c3);
                score += this.ts_(this.UC4__, c4);
                score += this.ts_(this.UC5__, c5);
                score += this.ts_(this.UC6__, c6);
                score += this.ts_(this.BC1__, c2 + c3);
                score += this.ts_(this.BC2__, c3 + c4);
                score += this.ts_(this.BC3__, c4 + c5);
                score += this.ts_(this.TC1__, c1 + c2 + c3);
                score += this.ts_(this.TC2__, c2 + c3 + c4);
                score += this.ts_(this.TC3__, c3 + c4 + c5);
                score += this.ts_(this.TC4__, c4 + c5 + c6);
                //score += this.ts_(this.TC5__, c4 + c5 + c6);
                score += this.ts_(this.UQ1__, p1 + c1);
                score += this.ts_(this.UQ2__, p2 + c2);
                score += this.ts_(this.UQ1__, p3 + c3);
                score += this.ts_(this.BQ1__, p2 + c2 + c3);
                score += this.ts_(this.BQ2__, p2 + c3 + c4);
                score += this.ts_(this.BQ3__, p3 + c2 + c3);
                score += this.ts_(this.BQ4__, p3 + c3 + c4);
                score += this.ts_(this.TQ1__, p2 + c1 + c2 + c3);
                score += this.ts_(this.TQ2__, p2 + c2 + c3 + c4);
                score += this.ts_(this.TQ3__, p3 + c1 + c2 + c3);
                score += this.ts_(this.TQ4__, p3 + c2 + c3 + c4);
                string p = "O";
                if (score > 0)
                {
                    result.Add(word);
                    word = "";
                    p = "B";
                }
                p1 = p2;
                p2 = p3;
                p3 = p;
                word += seg[i];
            }
            result.Add(word);

            return result.ToArray();
        }

        #endregion


        Dictionary<string, List<WordDicData>> wordDic = new Dictionary<string, List<WordDicData>>();
        List<string> wordList = new List<string>();
        List<string> learnWordList = new List<string>();

        public List<string> LearnWordList
        {
            get { return learnWordList; }
        }
        public void AddWordDic(string word)
        {
            wordList.Add(word);
            wordDic = null;
        }
        public void ClearWordDic()
        {
            wordDic = null;
            wordList.Clear();
        }
        public void AddRangeWordDic(IEnumerable<string> words)
        {
            wordList.AddRange(words.Select(n=>n.Split('\t').First()));
            wordDic = null;
        }
        public List<string> WordDicList
        {
            get { return wordList; }
            set { wordList = value; wordDic = null; }
        }

        public EventHandler LearnEnd;

        public struct WordDicData
        {
            string word;

            public string Word
            {
                get { return word; }
                set { word = value; }
            }
            int length; 

            public int Length
            {
                get { return length; }
                set { length = value; }
            }
            string replace ;

            public string Replace
            {
                get { return replace; }
                set { replace = value; }
            }


            //public string Word { get; set; }
            //public int Length { get; set; }
            //public string Replace { get; set; }

            ////public WordDicData()
            //{
            //    Replace = string.Empty;
            //}

            public static WordDicData Create(string text)
            {
                var dd = new WordDicData();
                var d = text.Split('\t');
                dd.Word = d[0];
                dd.Replace = string.Empty;
                if (d.Length > 1) dd.Replace = d[1];
                dd.Length = dd.Word.Length;
                return dd;
            }
            public static WordDicData Create(string word,string replace)
            {
                var dd = new WordDicData();
                dd.Word = word;
                dd.Replace = replace;
                dd.Length = word.Length;
                return dd;
            }

            public bool IsEmpty()
            {
                if (this.Word != null) return false;
                return true;
            }

            public static WordDicData GetEmpty()
            {
                return new WordDicData();
            }

            public string GetWord()
            {
                if (string.IsNullOrEmpty(Replace)) return Word;
                return Replace;
            }

            public bool EqualsText(string text)
            {
                if (text.Length >= Length)
                {
                    if (text.Substring(0, Length) == Word)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 辞書をの作成。
        /// </summary>
        private void CreateDic()
        {
            var list = new List<string>().Adds(learnWordList).Adds(wordList).Where(n=>n.Length>1).OrderBy(n => n.Substring(0, 2)).ThenByDescending(n => n.Length).Distinct().ToList();
            wordDic = list.GroupBy(n => n.Substring(0, 2)).Select(n => new { Key = n.Key, List = n.Select(m =>WordDicData.Create(m)).OrderByDescending(m => m.Length).ThenByDescending(m=>m.Replace.Length).ToList() }).ToDictionary(n => n.Key, n => n.List);
        }

        private WordDicData SearchDic(string head, string text)
        {
            if (wordDic == null) CreateDic();


            char firstChar = text.First();
            if(firstChar == ' ')
            {
                return new WordDicData() { Word = " ", Replace = string.Empty, Length = 1 };
            }
            if (endChar.Contains(firstChar)) return WordDicData.Create(firstChar.ToString());

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //カタカナ連続
            foreach (var item in text)
            {
                if (katakana.Contains(item)) sb.Append(item);
                else break;
            }
            if (sb.Length > 0) return WordDicData.Create(sb.ToString());


            //数字の連続判別
            foreach (var item in text)
            {
                if (numChar.Contains(item)) sb.Append(item);
                else break;
            }
            if (sb.Length > 0) return WordDicData.Create(sb.ToString());
            //アルファベットの連続
            foreach (var item in text)
            {
                if (alfabet.Contains(item)) sb.Append(item);
                else break;
            }
            if (sb.Length > 0) return WordDicData.Create(sb.ToString());

            //辞書を使う
            if (wordDic.ContainsKey(head))
            {
                var worddata = wordDic[head].DefaultIfEmpty(new WordDicData()).FirstOrDefault(n => n.EqualsText(text));
                if (worddata.IsEmpty() == false) return worddata;
            }


            //括弧閉じ
            int カッコ数 = 0;
            foreach (var item in text)
            {
                if (startカッコ.Contains(item))
                {
                    カッコ数++;
                }
                if(endカッコ.Contains(item))
                {
                    カッコ数--;
                }
                if (カッコ数 > 0) sb.Append(item);
                else
                {
                    if (sb.Length > 0) sb.Append(item);
                    break;
                }
            }
            if (sb.Length > 0 && sb.Length<16) return WordDicData.Create(sb.ToString());



            return WordDicData.GetEmpty();
        }

        System.Text.RegularExpressions.Regex hiragana = new System.Text.RegularExpressions.Regex("^[ぁ-ん。、]$");
        System.Text.RegularExpressions.Regex numRegex = new System.Text.RegularExpressions.Regex("^[ぁ-ん。、]$");
        HashSet<char> numChar = new HashSet<char>("+,-./0123456789:");
        HashSet<char> startカッコ = new HashSet<char>("([（【");
        HashSet<char> endカッコ = new HashSet<char>(")]）】");
        HashSet<char> katakana = new HashSet<char>("アィイゥウェエォオカガキギクグケゲコゴサザシジスズセゼソゾタダチヂッツヅテデトドナニヌネノハバパヒビピフブプヘベペホボポマミムメモャヤュユョヨラリルレロヮワヰヱヲンヴヵヶー");
        HashSet<char> endChar = new HashSet<char>("!?！？、。「」『』:");
        HashSet<char> alfabet = new HashSet<char>("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");

        public IEnumerable<string> Ngram(IEnumerable<string> list, int n, string separeter)
        {            
            List<List<string>> list2 = new List<List<string>>();
            List<string> l = new List<string>();
            list2.Add(l);
            foreach (var item in list)
            {
                if(endChar.Contains(item.First()))
                {
                    l = new List<string>();
                    list2.Add(l);
                }
                else
                {
                    var text = item.Trim();
                    if (text.Length > 0)
                    {
                        l.Add(text);
                    }
                }
            }
            foreach (var item in list2)
            {
                var d = item.ToArray();
                if (d.Length > n)
                {
                    for (int i = 0; i < d.Length - n + 1; i++)
                    {
                        //初めと最後がひらがな1文字の時は、排除。
                        if (hiragana.Match(d[i]).Success == false && hiragana.Match(d[i + n - 1]).Success == false)
                        {
                            yield return d.Skip(i).Take(n).JoinText(separeter).Trim();
                        }
                    }
                }                
            }
        }

        /// <summary>
        /// SegmentExtedをして、NGramの結果を返す
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxGram"></param>
        /// <returns></returns>
        public IEnumerable<string> GetNgram(string text,int maxGram)
        {
            List<string> list = new List<string>();
            var d = this.SegmentExted(text);
            for (int i = 2; i <= maxGram; i++)
            {
                list.AddRange(Ngram(d, i, string.Empty).Distinct().Where(n => n.Contains(" ") == false));
            }
            return list;
        }

        /// <summary>
        /// SegmentExtedをして、NGramの結果を返す
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxGram"></param>
        /// <returns></returns>
        public IEnumerable<string> GetNgram(string text, int[] nList)
        {
            List<string> list = new List<string>();
            var d = this.SegmentExted(text);
            foreach (var item in nList)
            {
                list.AddRange(Ngram(d, item, string.Empty).Distinct().Where(n => n.Contains(" ") == false));
            }

            return list;
        }


        /// <summary>
        /// 入力テキストを、分かち書きをして、Ngramで組み合わせを作って、指定割合以上のものを採用する。
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="rate"></param>
        public void Learning(IEnumerable<string> lines,double rate)
        {
            List<string> list = new List<string>();
            LearnWordList.Clear();
            CreateDic();
            foreach (var item in lines)
            {
                var d = SegmentExted(item);

                list.AddRange(Ngram(d, 2, string.Empty).Distinct());
                list.AddRange(Ngram(d, 3, string.Empty).Distinct());
                list.AddRange(Ngram(d, 4, string.Empty).Distinct());
                list.AddRange(Ngram(d, 5, string.Empty).Distinct());
                list.AddRange(Ngram(d, 6, string.Empty).Distinct());
            }
            var all = (double)lines.Count();
           
            var countList= list.GroupBy(n=>n).Select(n=>new {Word = n.Key,Rate = n.Count()/all});
            learnWordList = countList.Where(n => n.Rate >= rate).Select(n => n.Word).ToList();
            CreateDic();
            if (LearnEnd != null) LearnEnd(this,new EventArgs());
        }

        /// <summary>
        /// 入力テキストを、分かち書きをして、Ngramで組み合わせを作って、指定割合以上のものを採用する。
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="rate"></param>
        public void Learning(IEnumerable<string> lines, double rate,int ngram)
        {
            List<string> list = new List<string>();
            LearnWordList.Clear();
            CreateDic();
            foreach (var item in lines)
            {
                var d = SegmentExted(item);
                for (int i = 2; i <= ngram; i++)
                {
                    list.AddRange(Ngram(d, i, string.Empty).Distinct());
                }
            }
            var all = (double)lines.Count();

            var countList = list.GroupBy(n => n).Select(n => new { Word = n.Key, Rate = n.Count() / all });
            learnWordList = countList.Where(n => n.Rate >= rate).Select(n => n.Word).ToList();
            CreateDic();
            if (LearnEnd != null) LearnEnd(this, new EventArgs());
        }

        /// <summary>
        /// 入力テキストを、分かち書きをして、Ngramで組み合わせを作って、指定割合以上のものを採用する。(並列版)
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="rate">閾値とする出現確率</param>
        /// <param name="maxNgram">最大のNgram数</param>
        /// <param name="threadNum">スレッド数</param>
        public void LearningParallel(IEnumerable<string> lines, double rate,int take,int maxNgram,Action<int> progressAction)
        {
            LearnWordList.Clear();
            CreateDic();
            var c = (double)lines.Count();
            var dic = lines.ParalellCount(n => {
                var d = SegmentExted(n);
                List<string> l = new List<string>();
                for (int i = 2; i <= maxNgram; i++)
                {
                    l.AddRange(Ngram(d, i, string.Empty).Distinct());
                }
                return l;
            }, (int)c / 60, Environment.ProcessorCount, progressAction);
            learnWordList = dic.OrderByDescending(n=>n.Value).Take(take).Where(n => n.Value / c >= rate).Select(n => n.Key).ToList();
            CreateDic();
            if (LearnEnd != null) LearnEnd(this, new EventArgs());
        }



        public Func<string, PreSegment> PreSegmentFunc { get; set; }

        public IEnumerable<string> SegmentExted(string input)
        {
            if (string.IsNullOrEmpty(input))
                return new string[] { };
            List<string> result = new List<string>();

            if(PreSegmentFunc !=null)
            {
                var r = PreSegmentFunc(input);
                input = r.Text;
                result.AddRange(r.List);
            }

            List<string> seg = new List<string>();
            seg.Add("B3");
            seg.Add("B2");
            seg.Add("B1");
            List<string> ctype = new List<string>();
            ctype.Add("O");
            ctype.Add("O");
            ctype.Add("O");
            //char[] o = input.ToCharArray();
            for (int i = 0; i < input.Length; i++)
            {
                string c = input[i].ToString();
                seg.Add(c);
                ctype.Add(this.ctype_(c));
            }
            seg.Add("E1");
            seg.Add("E2");
            seg.Add("E3");
            ctype.Add("O");
            ctype.Add("O");
            ctype.Add("O");
            string word = seg[3];
            string p1 = "U";
            string p2 = "U";
            string p3 = "U";

            int postion = 0;
            while (input.Length > postion +1)
            {
                var i = postion + 4;
                WordDicData worddicdata = WordDicData.GetEmpty(); 
                if (postion + 2 < input.Length)
                {
                    worddicdata = SearchDic(input.Substring(postion, 2), input.Substring(postion));
                }
                if (worddicdata.IsEmpty()==false)
                {
                    var w = worddicdata.GetWord();
                    if (w.Length > 0) result.Add(w);
                    postion += worddicdata.Length;
                    i = postion + 4;
                    word = seg[i-1];
                    p1 = "O";
                    p2 = "O";
                    p3 = "B";
                }
                else
                {
                    int score = this.BIAS__;

                    string w1 = seg[i - 3];
                    string w2 = seg[i - 2];
                    string w3 = seg[i - 1];
                    string w4 = seg[i];
                    string w5 = seg[i + 1];
                    string w6 = seg[i + 2];
                    string c1 = ctype[i - 3];
                    string c2 = ctype[i - 2];
                    string c3 = ctype[i - 1];
                    string c4 = ctype[i];
                    string c5 = ctype[i + 1];
                    string c6 = ctype[i + 2];
                    score += this.ts_(this.UP1__, p1);
                    score += this.ts_(this.UP2__, p2);
                    score += this.ts_(this.UP3__, p3);
                    score += this.ts_(this.BP1__, p1 + p2);
                    score += this.ts_(this.BP2__, p2 + p3);
                    score += this.ts_(this.UW1__, w1);
                    score += this.ts_(this.UW2__, w2);
                    score += this.ts_(this.UW3__, w3);
                    score += this.ts_(this.UW4__, w4);
                    score += this.ts_(this.UW5__, w5);
                    score += this.ts_(this.UW6__, w6);
                    score += this.ts_(this.BW1__, w2 + w3);
                    score += this.ts_(this.BW2__, w3 + w4);
                    score += this.ts_(this.BW3__, w4 + w5);
                    score += this.ts_(this.TW1__, w1 + w2 + w3);
                    score += this.ts_(this.TW2__, w2 + w3 + w4);
                    score += this.ts_(this.TW3__, w3 + w4 + w5);
                    score += this.ts_(this.TW4__, w4 + w5 + w6);
                    score += this.ts_(this.UC1__, c1);
                    score += this.ts_(this.UC2__, c2);
                    score += this.ts_(this.UC3__, c3);
                    score += this.ts_(this.UC4__, c4);
                    score += this.ts_(this.UC5__, c5);
                    score += this.ts_(this.UC6__, c6);
                    score += this.ts_(this.BC1__, c2 + c3);
                    score += this.ts_(this.BC2__, c3 + c4);
                    score += this.ts_(this.BC3__, c4 + c5);
                    score += this.ts_(this.TC1__, c1 + c2 + c3);
                    score += this.ts_(this.TC2__, c2 + c3 + c4);
                    score += this.ts_(this.TC3__, c3 + c4 + c5);
                    score += this.ts_(this.TC4__, c4 + c5 + c6);
                    //score += this.ts_(this.TC5__, c4 + c5 + c6);
                    score += this.ts_(this.UQ1__, p1 + c1);
                    score += this.ts_(this.UQ2__, p2 + c2);
                    score += this.ts_(this.UQ1__, p3 + c3);
                    score += this.ts_(this.BQ1__, p2 + c2 + c3);
                    score += this.ts_(this.BQ2__, p2 + c3 + c4);
                    score += this.ts_(this.BQ3__, p3 + c2 + c3);
                    score += this.ts_(this.BQ4__, p3 + c3 + c4);
                    score += this.ts_(this.TQ1__, p2 + c1 + c2 + c3);
                    score += this.ts_(this.TQ2__, p2 + c2 + c3 + c4);
                    score += this.ts_(this.TQ3__, p3 + c1 + c2 + c3);
                    score += this.ts_(this.TQ4__, p3 + c2 + c3 + c4);
                    string p = "O";
                    if (score > 0)
                    {
                        result.Add(word);
                        word = "";
                        p = "B";
                    }
                    p1 = p2;
                    p2 = p3;
                    p3 = p;
                    word += seg[i];
                    postion++;
                }
            }
            if (word != "E1")
            {
                result.Add(word);
            }
            return result;
        }
    }

    /// <summary>
    /// 前処理用のクラス。メソッドチェーン
    /// </summary>
    public class PreSegment
    {
        public string Text { get; set; }
        public List<string> List { get; set; }

        public PreSegment()
        { }

        public PreSegment(string text)
        {
            this.Text = text;
            this.List = new List<string>();
        }

        public PreSegment(string text,IEnumerable<string> list)
        {
            this.Text = text;
            this.List = new List<string>(list);
        }

        public PreSegment TakeUrl()
        {
            return TakeRegex(@"https?://[\w/:%#\$&\?\(\)~\.=\+\-]+");
        }
        public PreSegment SkipUrl()
        {
            return SkipRegex(@"https?://[\w/:%#\$&\?\(\)~\.=\+\-]+");
        }
        public PreSegment TakeMention()
        {
            return TakeRegex(@"@\w*");
        }
        public PreSegment SkipMention()
        {
            return SkipRegex(@"@\w*");
        }

        public PreSegment SkipRegex(string pattern)
        {
            System.Text.RegularExpressions.Regex r = new Regex(pattern);
            List<string> list = new List<string>();
            foreach (System.Text.RegularExpressions.Match item in r.Matches(this.Text))
            {
                list.Add(item.Value);
            }
            foreach (var item in list)
            {
               // List.Add(item);
                Text = Text.Replace(item, " ");
            }
            return this;
        }

        public PreSegment TakeRegex(string pattern)
        {
            System.Text.RegularExpressions.Regex r = new Regex(pattern);
            List<string> list = new List<string>();
            foreach(System.Text.RegularExpressions.Match item in r.Matches(  this.Text))
            {
                list.Add(item.Value);
            }
            foreach (var item in list)
            {
                List.Add(item);
                Text = Text.Replace(item, " ");
            }
            return this;
        }

        /// <summary>
        /// すべてを小文字化します。
        /// </summary>
        /// <returns></returns>
        public PreSegment ToLower()
        {
            this.Text = this.Text.ToLower();
            return this;
        }

        /// <summary>
        /// 日本語の基本的な前処理
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public PreSegment PreprocessingForJapanese()
        {
            var t = Text;
            t = CSharp.Japanese.Kanaxs.Kana.ToHankaku(t);
            t = CSharp.Japanese.Kanaxs.Kana.ToZenkakuKana(t);
            t = CSharp.Japanese.Kanaxs.Kana.ToPadding(t);
        //    t = RawlerLib.Web.SimpleHtmlEncode(t);
            t = t.Replace("～", "ー").Replace("－", "ー").Replace("─", "ー");
            this.Text = t;
            return this;
        }
    }

}
