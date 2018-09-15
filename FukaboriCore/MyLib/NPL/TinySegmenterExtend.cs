using RawlerLib.MyExtend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Rawler.RawlerLib
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


        private void Init()
        {
            this.BC1__ = new Dictionary<string, int>() { { "HH", 5 }, { "II", 2461 }, { "KH", 405 }, { "OH", -1378 } };
            this.BC2__ = new Dictionary<string, int>() { { "AA", -3267 }, { "AI", 2744 }, { "AN", -878 }, { "HH", -4070 }, { "HM", -1711 }, { "HN", 4012 }, { "HO", 3761 }, { "IA", 1327 }, { "IH", -1184 }, { "II", -1332 }, { "IK", 1721 }, { "IO", 5492 }, { "KI", 3831 }, { "KK", -8741 }, { "MH", -3132 }, { "MK", 3334 }, { "OO", -2920 } };
            this.BC3__ = new Dictionary<string, int>() { { "HH", 995 }, { "HI", 625 }, { "HK", -721 }, { "HN", -1307 }, { "HO", -835 }, { "IH", -301 }, { "KK", 2762 }, { "MK", 1079 }, { "MM", 4034 }, { "OA", -1652 }, { "OH", 266 } };
            this.BP1__ = new Dictionary<string, int>() { { "BB", 295 }, { "OB", 304 }, { "OO", -125 }, { "UB", 352 } };
            this.BP2__ = new Dictionary<string, int>() { { "BO", 60 }, { "OO", -1762 } };
            this.BQ1__ = new Dictionary<string, int>() { { "BHH", 1150 }, { "BHM", 1521 }, { "BII", -1158 }, { "BIM", 885 }, { "BMH", 1208 }, { "BNH", 449 }, { "BOH", -91 }, { "BOO", -2597 }, { "OHI", 451 }, { "OIH", -295 }, { "OKA", 1851 }, { "OKH", -1020 }, { "OKK", 904 }, { "OOO", 2965 } };
            this.BQ2__ = new Dictionary<string, int>() { { "BHH", 118 }, { "BHI", -1159 }, { "BHM", 465 }, { "BIH", -919 }, { "BKK", -1720 }, { "BKO", 864 }, { "OHH", -1139 }, { "OHM", -181 }, { "OIH", 153 }, { "UHI", -1146 } };
            this.BQ3__ = new Dictionary<string, int>() { { "BHH", -792 }, { "BHI", 2664 }, { "BII", -299 }, { "BKI", 419 }, { "BMH", 937 }, { "BMM", 8335 }, { "BNN", 998 }, { "BOH", 775 }, { "OHH", 2174 }, { "OHM", 439 }, { "OII", 280 }, { "OKH", 1798 }, { "OKI", -793 }, { "OKO", -2242 }, { "OMH", -2402 }, { "OOO", 11699 } };
            this.BQ4__ = new Dictionary<string, int>() { { "BHH", -3895 }, { "BIH", 3761 }, { "BII", -4654 }, { "BIK", 1348 }, { "BKK", -1805 }, { "BMI", -3385 }, { "BOO", -12395 }, { "OAH", 925 }, { "OHH", 265 }, { "OHK", -2035 }, { "ONN", -973 } };
            this.BW1__ = new Dictionary<string, int>() { { ",と", 660 }, { ",同", 727 }, { "B1あ", 1404 }, { "B1同", 542 }, { "、と", 660 }, { "、同", 727 }, { "」と", 1682 }, { "あっ", 1505 }, { "いう", 1743 }, { "いっ", -2055 }, { "いる", 672 }, { "うし", -4817 }, { "うん", 665 }, { "から", 3472 }, { "がら", 600 }, { "こう", -790 }, { "こと", 2083 }, { "こん", -1262 }, { "さら", -4143 }, { "さん", 4573 }, { "した", 2641 }, { "して", 1104 }, { "すで", -3399 }, { "そこ", 1977 }, { "それ", -871 }, { "たち", 1122 }, { "ため", 601 }, { "った", 3463 }, { "つい", -802 }, { "てい", 805 }, { "てき", 1249 }, { "でき", 1127 }, { "です", 3445 }, { "では", 844 }, { "とい", -4915 }, { "とみ", 1922 }, { "どこ", 3887 }, { "ない", 5713 }, { "なっ", 3015 }, { "など", 7379 }, { "なん", -1113 }, { "にし", 2468 }, { "には", 1498 }, { "にも", 1671 }, { "に対", -912 }, { "の一", -501 }, { "の中", 741 }, { "ませ", 2448 }, { "まで", 1711 }, { "まま", 2600 }, { "まる", -2155 }, { "やむ", -1947 }, { "よっ", -2565 }, { "れた", 2369 }, { "れで", -913 }, { "をし", 1860 }, { "を見", 731 }, { "亡く", -1885 }, { "京都", 2558 }, { "取り", -2784 }, { "大き", -2604 }, { "大阪", 1497 }, { "平方", -2314 }, { "引き", -1335 }, { "日本", -195 }, { "本当", -2423 }, { "毎日", -2113 }, { "目指", -724 }, { "Ｂ１あ", 1404 }, { "Ｂ１同", 542 }, { "｣と", 1682 } };
            this.BW2__ = new Dictionary<string, int>() { { "..", -11822 }, { "11", -669 }, { "――", -5730 }, { "−−", -13175 }, { "いう", -1609 }, { "うか", 2490 }, { "かし", -1350 }, { "かも", -602 }, { "から", -7194 }, { "かれ", 4612 }, { "がい", 853 }, { "がら", -3198 }, { "きた", 1941 }, { "くな", -1597 }, { "こと", -8392 }, { "この", -4193 }, { "させ", 4533 }, { "され", 13168 }, { "さん", -3977 }, { "しい", -1819 }, { "しか", -545 }, { "した", 5078 }, { "して", 972 }, { "しな", 939 }, { "その", -3744 }, { "たい", -1253 }, { "たた", -662 }, { "ただ", -3857 }, { "たち", -785 }, { "たと", 1224 }, { "たは", -939 }, { "った", 4589 }, { "って", 1647 }, { "っと", -2094 }, { "てい", 6144 }, { "てき", 3640 }, { "てく", 2551 }, { "ては", -3110 }, { "ても", -3065 }, { "でい", 2665 }, { "でき", -1528 }, { "でし", -3828 }, { "です", -4761 }, { "でも", -4203 }, { "とい", 1890 }, { "とこ", -1745 }, { "とと", -2279 }, { "との", 720 }, { "とみ", 5168 }, { "とも", -3941 }, { "ない", -2488 }, { "なが", -1313 }, { "など", -6509 }, { "なの", 2614 }, { "なん", 3099 }, { "にお", -1615 }, { "にし", 2748 }, { "にな", 2454 }, { "によ", -7235 }, { "に対", -14943 }, { "に従", -4688 }, { "に関", -11388 }, { "のか", 2093 }, { "ので", -7059 }, { "のに", -6041 }, { "のの", -6125 }, { "はい", 1073 }, { "はが", -1033 }, { "はず", -2532 }, { "ばれ", 1813 }, { "まし", -1315 }, { "まで", -6621 }, { "まれ", 5409 }, { "めて", -3153 }, { "もい", 2230 }, { "もの", -10713 }, { "らか", -944 }, { "らし", -1611 }, { "らに", -1897 }, { "りし", 651 }, { "りま", 1620 }, { "れた", 4270 }, { "れて", 849 }, { "れば", 4114 }, { "ろう", 6067 }, { "われ", 7901 }, { "を通", -11877 }, { "んだ", 728 }, { "んな", -4115 }, { "一人", 602 }, { "一方", -1375 }, { "一日", 970 }, { "一部", -1051 }, { "上が", -4479 }, { "会社", -1115 }, { "出て", 2163 }, { "分の", -7758 }, { "同党", 970 }, { "同日", -913 }, { "大阪", -2471 }, { "委員", -1250 }, { "少な", -1050 }, { "年度", -8669 }, { "年間", -1625 }, { "府県", -2363 }, { "手権", -1982 }, { "新聞", -4065 }, { "日新", -722 }, { "日本", -7068 }, { "日米", 3372 }, { "曜日", -601 }, { "朝鮮", -2355 }, { "本人", -2697 }, { "東京", -1543 }, { "然と", -1384 }, { "社会", -1275 }, { "立て", -990 }, { "第に", -1612 }, { "米国", -4268 }, { "１１", -669 } };
            this.BW3__ = new Dictionary<string, int>() { { "あた", -2194 }, { "あり", 719 }, { "ある", 3845 }, { "い.", -1185 }, { "い。", -1185 }, { "いい", 5308 }, { "いえ", 2079 }, { "いく", 3029 }, { "いた", 2055 }, { "いっ", 1883 }, { "いる", 5600 }, { "いわ", 1527 }, { "うち", 1117 }, { "うと", 4798 }, { "えと", 1454 }, { "か.", 2857 }, { "か。", 2857 }, { "かけ", -743 }, { "かっ", -4098 }, { "かに", -669 }, { "から", 6520 }, { "かり", -2670 }, { "が,", 1815 }, { "が、", 1815 }, { "がき", -4855 }, { "がけ", -1127 }, { "がっ", -913 }, { "がら", -4977 }, { "がり", -2064 }, { "きた", 1645 }, { "けど", 1374 }, { "こと", 7397 }, { "この", 1542 }, { "ころ", -2757 }, { "さい", -714 }, { "さを", 975 }, { "し,", 1557 }, { "し、", 1557 }, { "しい", -3714 }, { "した", 3562 }, { "して", 1449 }, { "しな", 2608 }, { "しま", 1200 }, { "す.", -1310 }, { "す。", -1310 }, { "する", 6521 }, { "ず,", 3425 }, { "ず、", 3425 }, { "ずに", 841 }, { "そう", 428 }, { "た.", 8875 }, { "た。", 8875 }, { "たい", -594 }, { "たの", 812 }, { "たり", -1183 }, { "たる", -853 }, { "だ.", 4098 }, { "だ。", 4098 }, { "だっ", 1004 }, { "った", -4748 }, { "って", 300 }, { "てい", 6240 }, { "てお", 855 }, { "ても", 302 }, { "です", 1437 }, { "でに", -1482 }, { "では", 2295 }, { "とう", -1387 }, { "とし", 2265 }, { "との", 541 }, { "とも", -3543 }, { "どう", 4664 }, { "ない", 1795 }, { "なく", -903 }, { "など", 2135 }, { "に,", -1021 }, { "に、", -1021 }, { "にし", 1771 }, { "にな", 1905 }, { "には", 2644 }, { "の,", -724 }, { "の、", -724 }, { "の子", -1000 }, { "は,", 1337 }, { "は、", 1337 }, { "べき", 2181 }, { "まし", 1113 }, { "ます", 6943 }, { "まっ", -1549 }, { "まで", 6154 }, { "まれ", -793 }, { "らし", 1479 }, { "られ", 6820 }, { "るる", 3818 }, { "れ,", 854 }, { "れ、", 854 }, { "れた", 1850 }, { "れて", 1375 }, { "れば", -3245 }, { "れる", 1091 }, { "われ", -605 }, { "んだ", 605 }, { "んで", 798 }, { "カ月", 990 }, { "会議", 860 }, { "入り", 1232 }, { "大会", 2217 }, { "始め", 1681 }, { "市", 965 }, { "新聞", -5055 }, { "日,", 974 }, { "日、", 974 }, { "社会", 2024 }, { "ｶ月", 990 } };
            this.TC1__ = new Dictionary<string, int>() { { "AAA", 1093 }, { "HHH", 1029 }, { "HHM", 580 }, { "HII", 998 }, { "HOH", -390 }, { "HOM", -331 }, { "IHI", 1169 }, { "IOH", -142 }, { "IOI", -1015 }, { "IOM", 467 }, { "MMH", 187 }, { "OOI", -1832 } };
            this.TC2__ = new Dictionary<string, int>() { { "HHO", 2088 }, { "HII", -1023 }, { "HMM", -1154 }, { "IHI", -1965 }, { "KKH", 703 }, { "OII", -2649 } };
            this.TC3__ = new Dictionary<string, int>() { { "AAA", -294 }, { "HHH", 345 }, { "HHI", -341 }, { "HII", -1088 }, { "HIK", 731 }, { "HOH", -1485 }, { "IHH", 128 }, { "IHI", -3041 }, { "IHO", -1935 }, { "IIH", -825 }, { "IIM", -1035 }, { "IOI", -542 }, { "KHH", -1215 }, { "KKA", 491 }, { "KKH", -1217 }, { "KOK", -1009 }, { "MHH", -2694 }, { "MHM", -457 }, { "MHO", 123 }, { "MMH", -471 }, { "NNH", -1689 }, { "NNO", 662 }, { "OHO", -3393 } };
            this.TC4__ = new Dictionary<string, int>() { { "HHH", -203 }, { "HHI", 1344 }, { "HHK", 365 }, { "HHM", -122 }, { "HHN", 182 }, { "HHO", 669 }, { "HIH", 804 }, { "HII", 679 }, { "HOH", 445 }, { "IHH", 695 }, { "IHO", -2324 }, { "IIH", 321 }, { "III", 1497 }, { "IIO", 655 }, { "IOO", 54 }, { "KAK", 4845 }, { "KKA", 3385 }, { "KKK", 3065 }, { "MHH", -405 }, { "MHI", 201 }, { "MMH", -241 }, { "MMM", 661 }, { "MOM", 841 } };
            this.TQ1__ = new Dictionary<string, int>() { { "BHHH", -227 }, { "BHHI", 315 }, { "BHIH", -132 }, { "BIHH", 60 }, { "BIII", 1595 }, { "BNHH", -744 }, { "BOHH", 225 }, { "BOOO", -908 }, { "OAKK", 482 }, { "OHHH", 281 }, { "OHIH", 249 }, { "OIHI", 200 }, { "OIIH", -68 } };
            this.TQ2__ = new Dictionary<string, int>() { { "BIHH", -1401 }, { "BIII", -1033 }, { "BKAK", -543 }, { "BOOO", -5591 } };
            this.TQ3__ = new Dictionary<string, int>() { { "BHHH", 478 }, { "BHHM", -1073 }, { "BHIH", 222 }, { "BHII", -504 }, { "BIIH", -115 }, { "BIII", -105 }, { "BMHI", -863 }, { "BMHM", -464 }, { "BOMH", 620 }, { "OHHH", 345 }, { "OHHI", 1729 }, { "OHII", 997 }, { "OHMH", 481 }, { "OIHH", 623 }, { "OIIH", 1344 }, { "OKAK", 2792 }, { "OKHH", 587 }, { "OKKA", 679 }, { "OOHH", 110 }, { "OOII", -685 } };
            this.TQ4__ = new Dictionary<string, int>() { { "BHHH", -721 }, { "BHHM", -3604 }, { "BHII", -965 }, { "BIIH", -607 }, { "BIII", -2181 }, { "OAAA", -2763 }, { "OAKK", 180 }, { "OHHH", -294 }, { "OHHI", 2445 }, { "OHHO", 480 }, { "OHIH", -1573 }, { "OIHH", 1935 }, { "OIHI", -493 }, { "OIIH", 625 }, { "OIII", -4007 }, { "OKAK", -8156 } };
            this.TW1__ = new Dictionary<string, int>() { { "につい", -4681 }, { "東京都", 2026 } };
            this.TW2__ = new Dictionary<string, int>() { { "ある程", -2049 }, { "いった", -1255 }, { "ころが", -2434 }, { "しょう", 3873 }, { "その後", -4430 }, { "だって", -1049 }, { "ていた", 1833 }, { "として", -4657 }, { "ともに", -4517 }, { "もので", 1882 }, { "一気に", -792 }, { "初めて", -1512 }, { "同時に", -8097 }, { "大きな", -1255 }, { "対して", -2721 }, { "社会党", -3216 } };
            this.TW3__ = new Dictionary<string, int>() { { "いただ", -1734 }, { "してい", 1314 }, { "として", -4314 }, { "につい", -5483 }, { "にとっ", -5989 }, { "に当た", -6247 }, { "ので,", -727 }, { "ので、", -727 }, { "のもの", -600 }, { "れから", -3752 }, { "十二月", -2287 } };
            this.TW4__ = new Dictionary<string, int>() { { "いう.", 8575 }, { "いう。", 8575 }, { "からな", -2348 }, { "してい", 2958 }, { "たが,", 1515 }, { "たが、", 1515 }, { "ている", 1538 }, { "という", 1349 }, { "ました", 5543 }, { "ません", 1097 }, { "ようと", -4258 }, { "よると", 5865 } };
            this.UC1__ = new Dictionary<string, int>() { { "A", 484 }, { "K", 93 }, { "M", 645 }, { "O", -505 } };
            this.UC2__ = new Dictionary<string, int>() { { "A", 819 }, { "H", 1059 }, { "I", 409 }, { "M", 3987 }, { "N", 5775 }, { "O", 646 } };
            this.UC3__ = new Dictionary<string, int>() { { "A", -1370 }, { "I", 2311 } };
            this.UC4__ = new Dictionary<string, int>() { { "A", -2643 }, { "H", 1809 }, { "I", -1032 }, { "K", -3450 }, { "M", 3565 }, { "N", 3875 }, { "O", 6646 } };
            this.UC5__ = new Dictionary<string, int>() { { "H", 313 }, { "I", -1238 }, { "K", -799 }, { "M", 539 }, { "O", -831 } };
            this.UC6__ = new Dictionary<string, int>() { { "H", -505 }, { "I", -253 }, { "K", 87 }, { "M", 247 }, { "O", -387 } };
            this.UP1__ = new Dictionary<string, int>() { { "O", -214 } };
            this.UP2__ = new Dictionary<string, int>() { { "B", 69 }, { "O", 935 } };
            this.UP3__ = new Dictionary<string, int>() { { "B", 189 } };
            this.UQ1__ = new Dictionary<string, int>() { { "BH", 21 }, { "BI", -12 }, { "BK", -99 }, { "BN", 142 }, { "BO", -55 }, { "OH", -95 }, { "OI", 477 }, { "OK", 410 }, { "OO", -2422 } };
            this.UQ2__ = new Dictionary<string, int>() { { "BH", 215 }, { "BI", 113 }, { "OK", 1759 } };
            this.UQ3__ = new Dictionary<string, int>() { { "BA", -479 }, { "BH", 42 }, { "BI", 1913 }, { "BK", -7198 }, { "BM", 3160 }, { "BN", 6427 }, { "BO", 14761 }, { "OI", -827 }, { "ON", -3212 } };
            this.UW1__ = new Dictionary<string, int>() { { ",", 155 }, { "、", 155 }, { "「", -463 }, { "あ", -941 }, { "う", -127 }, { "が", -553 }, { "き", 121 }, { "こ", 505 }, { "で", -201 }, { "と", -547 }, { "ど", -123 }, { "に", -789 }, { "の", -185 }, { "は", -847 }, { "も", -465 }, { "や", -470 }, { "よ", 182 }, { "ら", -292 }, { "り", 208 }, { "れ", 169 }, { "を", -445 }, { "ん", -137 }, { "・", -135 }, { "主", -402 }, { "京", -268 }, { "区", -912 }, { "午", 871 }, { "国", -460 }, { "大", 561 }, { "委", 729 }, { "市", -411 }, { "日", -141 }, { "理", 361 }, { "生", -408 }, { "県", -385 }, { "都", -718 }, { "｢", -463 }, { "･", -135 } };
            this.UW2__ = new Dictionary<string, int>() { { ",", -829 }, { "、", -829 }, { "〇", 892 }, { "「", -645 }, { "」", 3145 }, { "あ", -538 }, { "い", 505 }, { "う", 134 }, { "お", -502 }, { "か", 1454 }, { "が", -855 }, { "く", -412 }, { "こ", 1141 }, { "さ", 878 }, { "ざ", 540 }, { "し", 1529 }, { "す", -675 }, { "せ", 300 }, { "そ", -1011 }, { "た", 188 }, { "だ", 1837 }, { "つ", -949 }, { "て", -291 }, { "で", -268 }, { "と", -981 }, { "ど", 1273 }, { "な", 1063 }, { "に", -1764 }, { "の", 130 }, { "は", -409 }, { "ひ", -1273 }, { "べ", 1261 }, { "ま", 600 }, { "も", -1263 }, { "や", -402 }, { "よ", 1639 }, { "り", -579 }, { "る", -694 }, { "れ", 571 }, { "を", -2515 }, { "ん", 2095 }, { "ア", -587 }, { "カ", 305 }, { "キ", 568 }, { "ッ", 831 }, { "三", -758 }, { "不", -2150 }, { "世", -302 }, { "中", -968 }, { "主", -861 }, { "事", 492 }, { "人", -123 }, { "会", 978 }, { "保", 362 }, { "入", 548 }, { "初", -3025 }, { "副", -1565 }, { "北", -3414 }, { "区", -422 }, { "大", -1769 }, { "天", -865 }, { "太", -483 }, { "子", -1519 }, { "学", 760 }, { "実", 1023 }, { "小", -2009 }, { "市", -813 }, { "年", -1060 }, { "強", 1067 }, { "手", -1519 }, { "揺", -1033 }, { "政", 1522 }, { "文", -1355 }, { "新", -1682 }, { "日", -1815 }, { "明", -1462 }, { "最", -630 }, { "朝", -1843 }, { "本", -1650 }, { "東", -931 }, { "果", -665 }, { "次", -2378 }, { "民", -180 }, { "気", -1740 }, { "理", 752 }, { "発", 529 }, { "目", -1584 }, { "相", -242 }, { "県", -1165 }, { "立", -763 }, { "第", 810 }, { "米", 509 }, { "自", -1353 }, { "行", 838 }, { "西", -744 }, { "見", -3874 }, { "調", 1010 }, { "議", 1198 }, { "込", 3041 }, { "開", 1758 }, { "間", -1257 }, { "｢", -645 }, { "｣", 3145 }, { "ｯ", 831 }, { "ｱ", -587 }, { "ｶ", 305 }, { "ｷ", 568 } };
            this.UW3__ = new Dictionary<string, int>() { { ",", 4889 }, { "1", -800 }, { "−", -1723 }, { "、", 4889 }, { "々", -2311 }, { "〇", 5827 }, { "」", 2670 }, { "〓", -3573 }, { "あ", -2695 }, { "い", 1005 }, { "う", 2342 }, { "え", 1983 }, { "お", -4864 }, { "か", -1163 }, { "が", 3271 }, { "く", 1004 }, { "け", 388 }, { "げ", 401 }, { "こ", -3552 }, { "ご", -3115 }, { "さ", -1058 }, { "し", -395 }, { "す", 584 }, { "せ", 3685 }, { "そ", -5228 }, { "た", 842 }, { "ち", -521 }, { "っ", -1444 }, { "つ", -1081 }, { "て", 6167 }, { "で", 2318 }, { "と", 1691 }, { "ど", -899 }, { "な", -2788 }, { "に", 2745 }, { "の", 4055 }, { "は", 4555 }, { "ひ", -2171 }, { "ふ", -1798 }, { "へ", 1199 }, { "ほ", -5515 }, { "ま", -4384 }, { "み", -120 }, { "め", 1205 }, { "も", 2323 }, { "や", -788 }, { "よ", -202 }, { "ら", 727 }, { "り", 649 }, { "る", 5905 }, { "れ", 2773 }, { "わ", -1207 }, { "を", 6620 }, { "ん", -518 }, { "ア", 551 }, { "グ", 1319 }, { "ス", 874 }, { "ッ", -1350 }, { "ト", 521 }, { "ム", 1109 }, { "ル", 1591 }, { "ロ", 2201 }, { "ン", 278 }, { "・", -3794 }, { "一", -1619 }, { "下", -1759 }, { "世", -2087 }, { "両", 3815 }, { "中", 653 }, { "主", -758 }, { "予", -1193 }, { "二", 974 }, { "人", 2742 }, { "今", 792 }, { "他", 1889 }, { "以", -1368 }, { "低", 811 }, { "何", 4265 }, { "作", -361 }, { "保", -2439 }, { "元", 4858 }, { "党", 3593 }, { "全", 1574 }, { "公", -3030 }, { "六", 755 }, { "共", -1880 }, { "円", 5807 }, { "再", 3095 }, { "分", 457 }, { "初", 2475 }, { "別", 1129 }, { "前", 2285 }, { "副", 4437 }, { "力", 365 }, { "動", -949 }, { "務", -1872 }, { "化", 1327 }, { "北", -1038 }, { "区", 4645 }, { "千", -2309 }, { "午", -783 }, { "協", -1005 }, { "口", 483 }, { "右", 1233 }, { "各", 3588 }, { "合", -241 }, { "同", 3905 }, { "和", -837 }, { "員", 4513 }, { "国", 642 }, { "型", 1389 }, { "場", 1219 }, { "外", -241 }, { "妻", 2015 }, { "学", -1355 }, { "安", -423 }, { "実", -1008 }, { "家", 1078 }, { "小", -513 }, { "少", -3102 }, { "州", 1155 }, { "市", 3197 }, { "平", -1804 }, { "年", 2415 }, { "広", -1030 }, { "府", 1605 }, { "度", 1452 }, { "建", -2352 }, { "当", -3885 }, { "得", 1905 }, { "思", -1291 }, { "性", 1822 }, { "戸", -488 }, { "指", -3973 }, { "政", -2013 }, { "教", -1479 }, { "数", 3222 }, { "文", -1489 }, { "新", 1764 }, { "日", 2099 }, { "旧", 5792 }, { "昨", -661 }, { "時", -1248 }, { "曜", -951 }, { "最", -937 }, { "月", 4125 }, { "期", 360 }, { "李", 3094 }, { "村", 364 }, { "東", -805 }, { "核", 5155 }, { "森", 2438 }, { "業", 484 }, { "氏", 2613 }, { "民", -1694 }, { "決", -1073 }, { "法", 1868 }, { "海", -495 }, { "無", 979 }, { "物", 461 }, { "特", -3850 }, { "生", -273 }, { "用", 914 }, { "町", 1215 }, { "的", 7313 }, { "直", -1835 }, { "省", 792 }, { "県", 6293 }, { "知", -1528 }, { "私", 4231 }, { "税", 401 }, { "立", -960 }, { "第", 1201 }, { "米", 7767 }, { "系", 3065 }, { "約", 3663 }, { "級", 1384 }, { "統", -4229 }, { "総", 1163 }, { "線", 1255 }, { "者", 6457 }, { "能", 725 }, { "自", -2869 }, { "英", 785 }, { "見", 1044 }, { "調", -562 }, { "財", -733 }, { "費", 1777 }, { "車", 1835 }, { "軍", 1375 }, { "込", -1504 }, { "通", -1135 }, { "選", -681 }, { "郎", 1025 }, { "郡", 4404 }, { "部", 1200 }, { "金", 2163 }, { "長", 421 }, { "開", -1432 }, { "間", 1302 }, { "関", -1282 }, { "雨", 2009 }, { "電", -1045 }, { "非", 2065 }, { "駅", 1620 }, { "１", -800 }, { "｣", 2670 }, { "･", -3794 }, { "ｯ", -1350 }, { "ｱ", 551 }, { "ｸﾞ", 1319 }, { "ｽ", 874 }, { "ﾄ", 521 }, { "ﾑ", 1109 }, { "ﾙ", 1591 }, { "ﾛ", 2201 }, { "ﾝ", 278 } };
            this.UW4__ = new Dictionary<string, int>() { { ",", 3930 }, { ".", 3508 }, { "―", -4841 }, { "、", 3930 }, { "。", 3508 }, { "〇", 4999 }, { "「", 1895 }, { "」", 3798 }, { "〓", -5155 }, { "あ", 4752 }, { "い", -3435 }, { "う", -640 }, { "え", -2514 }, { "お", 2405 }, { "か", 530 }, { "が", 6005 }, { "き", -4482 }, { "ぎ", -3821 }, { "く", -3788 }, { "け", -4375 }, { "げ", -4734 }, { "こ", 2255 }, { "ご", 1979 }, { "さ", 2864 }, { "し", -843 }, { "じ", -2505 }, { "す", -731 }, { "ず", 1251 }, { "せ", 181 }, { "そ", 4091 }, { "た", 5034 }, { "だ", 5408 }, { "ち", -3654 }, { "っ", -5882 }, { "つ", -1659 }, { "て", 3994 }, { "で", 7410 }, { "と", 4547 }, { "な", 5433 }, { "に", 6499 }, { "ぬ", 1853 }, { "ね", 1413 }, { "の", 7395 }, { "は", 8578 }, { "ば", 1940 }, { "ひ", 4249 }, { "び", -4134 }, { "ふ", 1345 }, { "へ", 6665 }, { "べ", -744 }, { "ほ", 1464 }, { "ま", 1051 }, { "み", -2082 }, { "む", -882 }, { "め", -5045 }, { "も", 4169 }, { "ゃ", -2665 }, { "や", 2795 }, { "ょ", -1544 }, { "よ", 3351 }, { "ら", -2922 }, { "り", -9725 }, { "る", -14895 }, { "れ", -2613 }, { "ろ", -4570 }, { "わ", -1783 }, { "を", 13150 }, { "ん", -2352 }, { "カ", 2145 }, { "コ", 1789 }, { "セ", 1287 }, { "ッ", -724 }, { "ト", -403 }, { "メ", -1635 }, { "ラ", -881 }, { "リ", -541 }, { "ル", -855 }, { "ン", -3637 }, { "・", -4371 }, { "ー", -11870 }, { "一", -2069 }, { "中", 2210 }, { "予", 782 }, { "事", -190 }, { "井", -1768 }, { "人", 1035 }, { "以", 544 }, { "会", 950 }, { "体", -1285 }, { "作", 530 }, { "側", 4292 }, { "先", 601 }, { "党", -2005 }, { "共", -1212 }, { "内", 584 }, { "円", 788 }, { "初", 1347 }, { "前", 1623 }, { "副", 3879 }, { "力", -302 }, { "動", -740 }, { "務", -2715 }, { "化", 775 }, { "区", 4517 }, { "協", 1013 }, { "参", 1555 }, { "合", -1834 }, { "和", -681 }, { "員", -910 }, { "器", -851 }, { "回", 1500 }, { "国", -619 }, { "園", -1200 }, { "地", 865 }, { "場", -1410 }, { "塁", -2094 }, { "士", -1413 }, { "多", 1067 }, { "大", 571 }, { "子", -4802 }, { "学", -1397 }, { "定", -1057 }, { "寺", -809 }, { "小", 1910 }, { "屋", -1328 }, { "山", -1500 }, { "島", -2055 }, { "川", -2667 }, { "市", 2771 }, { "年", 374 }, { "庁", -4555 }, { "後", 455 }, { "性", 553 }, { "感", 915 }, { "所", -1565 }, { "支", 855 }, { "改", 787 }, { "政", 2182 }, { "教", 704 }, { "文", 522 }, { "方", -855 }, { "日", 1798 }, { "時", 1829 }, { "最", 845 }, { "月", -9065 }, { "木", -485 }, { "来", -442 }, { "校", -360 }, { "業", -1043 }, { "氏", 5388 }, { "民", -2715 }, { "気", -910 }, { "沢", -939 }, { "済", -543 }, { "物", -735 }, { "率", 672 }, { "球", -1267 }, { "生", -1285 }, { "産", -1101 }, { "田", -2900 }, { "町", 1825 }, { "的", 2585 }, { "目", 922 }, { "省", -3485 }, { "県", 2997 }, { "空", -867 }, { "立", -2112 }, { "第", 788 }, { "米", 2937 }, { "系", 785 }, { "約", 2171 }, { "経", 1145 }, { "統", -1169 }, { "総", 940 }, { "線", -994 }, { "署", 749 }, { "者", 2145 }, { "能", -730 }, { "般", -852 }, { "行", -792 }, { "規", 792 }, { "警", -1184 }, { "議", -244 }, { "谷", -1000 }, { "賞", 730 }, { "車", -1481 }, { "軍", 1158 }, { "輪", -1433 }, { "込", -3370 }, { "近", 929 }, { "道", -1291 }, { "選", 2595 }, { "郎", -4865 }, { "都", 1192 }, { "野", -1100 }, { "銀", -2213 }, { "長", 357 }, { "間", -2344 }, { "院", -2297 }, { "際", -2604 }, { "電", -878 }, { "領", -1659 }, { "題", -792 }, { "館", -1984 }, { "首", 1749 }, { "高", 2120 }, { "｢", 1895 }, { "｣", 3798 }, { "･", -4371 }, { "ｯ", -724 }, { "ｰ", -11870 }, { "ｶ", 2145 }, { "ｺ", 1789 }, { "ｾ", 1287 }, { "ﾄ", -403 }, { "ﾒ", -1635 }, { "ﾗ", -881 }, { "ﾘ", -541 }, { "ﾙ", -855 }, { "ﾝ", -3637 } };
            this.UW5__ = new Dictionary<string, int>() { { ",", 465 }, { ".", -299 }, { "1", -514 }, { "E2", -32768 }, { "]", -2762 }, { "、", 465 }, { "。", -299 }, { "「", 363 }, { "あ", 1655 }, { "い", 331 }, { "う", -503 }, { "え", 1199 }, { "お", 527 }, { "か", 647 }, { "が", -421 }, { "き", 1624 }, { "ぎ", 1971 }, { "く", 312 }, { "げ", -983 }, { "さ", -1537 }, { "し", -1371 }, { "す", -852 }, { "だ", -1185 }, { "ち", 1093 }, { "っ", 52 }, { "つ", 921 }, { "て", -18 }, { "で", -850 }, { "と", -127 }, { "ど", 1682 }, { "な", -787 }, { "に", -1224 }, { "の", -635 }, { "は", -578 }, { "べ", 1001 }, { "み", 502 }, { "め", 865 }, { "ゃ", 3350 }, { "ょ", 854 }, { "り", -208 }, { "る", 429 }, { "れ", 504 }, { "わ", 419 }, { "を", -1264 }, { "ん", 327 }, { "イ", 241 }, { "ル", 451 }, { "ン", -343 }, { "中", -871 }, { "京", 722 }, { "会", -1153 }, { "党", -654 }, { "務", 3519 }, { "区", -901 }, { "告", 848 }, { "員", 2104 }, { "大", -1295 }, { "学", -548 }, { "定", 1785 }, { "嵐", -1304 }, { "市", -2991 }, { "席", 921 }, { "年", 1763 }, { "思", 872 }, { "所", -814 }, { "挙", 1618 }, { "新", -1682 }, { "日", 218 }, { "月", -4353 }, { "査", 932 }, { "格", 1355 }, { "機", -1508 }, { "氏", -1347 }, { "田", 240 }, { "町", -3912 }, { "的", -3149 }, { "相", 1319 }, { "省", -1052 }, { "県", -4003 }, { "研", -997 }, { "社", -278 }, { "空", -813 }, { "統", 1955 }, { "者", -2233 }, { "表", 663 }, { "語", -1073 }, { "議", 1219 }, { "選", -1018 }, { "郎", -368 }, { "長", 785 }, { "間", 1191 }, { "題", 2368 }, { "館", -689 }, { "１", -514 }, { "Ｅ２", -32768 }, { "｢", 363 }, { "ｲ", 241 }, { "ﾙ", 451 }, { "ﾝ", -343 } };
            this.UW6__ = new Dictionary<string, int>() { { ",", 227 }, { ".", 808 }, { "1", -270 }, { "E1", 305 }, { "、", 227 }, { "。", 808 }, { "あ", -307 }, { "う", 189 }, { "か", 241 }, { "が", -73 }, { "く", -121 }, { "こ", -200 }, { "じ", 1782 }, { "す", 383 }, { "た", -428 }, { "っ", 573 }, { "て", -1014 }, { "で", 101 }, { "と", -105 }, { "な", -253 }, { "に", -149 }, { "の", -417 }, { "は", -235 }, { "も", -205 }, { "り", 187 }, { "る", -135 }, { "を", 195 }, { "ル", -673 }, { "ン", -495 }, { "一", -277 }, { "中", 201 }, { "件", -800 }, { "会", 624 }, { "前", 302 }, { "区", 1792 }, { "員", -1212 }, { "委", 798 }, { "学", -960 }, { "市", 887 }, { "広", -695 }, { "後", 535 }, { "業", -697 }, { "相", 753 }, { "社", -507 }, { "福", 974 }, { "空", -822 }, { "者", 1811 }, { "連", 463 }, { "郎", 1082 }, { "１", -270 }, { "Ｅ１", 305 }, { "ﾙ", -673 }, { "ﾝ", -496 } };

        }
         #endregion

        #region オリジナルTinySegmenter
        private Dictionary<Regex, string> chartype_;

        public TinySegmenter()
        {
            Init();
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
            wordList.AddRange(words);
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
            string replace;

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
            public static WordDicData Create(string word, string replace)
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
            var list = new List<string>().Adds(learnWordList).Adds(wordList).Where(n => n.Length > 1).OrderBy(n => n.Substring(0, 2)).ThenByDescending(n => n.Length).Distinct().ToList();
            wordDic = list.GroupBy(n => n.Substring(0, 2)).Select(n => new { Key = n.Key, List = n.Select(m => WordDicData.Create(m)).OrderByDescending(m => m.Length).ThenByDescending(m => m.Replace.Length).ToList() }).ToDictionary(n => n.Key, n => n.List);
        }

        private WordDicData SearchDic(string head, string text)
        {
            if (wordDic == null) CreateDic();


            char firstChar = text.ToCharArray().First();
            if (firstChar == ' ')
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
                if (endカッコ.Contains(item))
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
            if (sb.Length > 0 && sb.Length < 16) return WordDicData.Create(sb.ToString());



            return WordDicData.GetEmpty();
        }

        System.Text.RegularExpressions.Regex hiragana = new System.Text.RegularExpressions.Regex("^[ぁ-ん。、]$");
        System.Text.RegularExpressions.Regex numRegex = new System.Text.RegularExpressions.Regex("^[ぁ-ん。、]$");
        HashSet<char> numChar = new HashSet<char>("+,-./0123456789:".ToCharArray());
        HashSet<char> startカッコ = new HashSet<char>("([（【".ToCharArray());
        HashSet<char> endカッコ = new HashSet<char>(")]）】".ToCharArray());
        HashSet<char> katakana = new HashSet<char>("アィイゥウェエォオカガキギクグケゲコゴサザシジスズセゼソゾタダチヂッツヅテデトドナニヌネノハバパヒビピフブプヘベペホボポマミムメモャヤュユョヨラリルレロヮワヰヱヲンヴヵヶー".ToCharArray());
        HashSet<char> endChar = new HashSet<char>("!?！？、。「」『』:".ToCharArray());
        HashSet<char> alfabet = new HashSet<char>("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());

        private IEnumerable<string> Ngram(IEnumerable<string> list, int n, string separeter)
        {
            List<List<string>> list2 = new List<List<string>>();
            List<string> l = new List<string>();
            list2.Add(l);
            foreach (var item in list)
            {
                if (endChar.Contains(item.ToCharArray().First()))
                {
                    l = new List<string>();
                    list2.Add(l);
                }
                else
                {
                    l.Add(item);
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
        public IEnumerable<string> GetNgram(string text, int maxGram)
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
        public void Learning(IEnumerable<string> lines, double rate)
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

            var countList = list.GroupBy(n => n).Select(n => new { Word = n.Key, Rate = n.Count() / all });
            learnWordList = countList.Where(n => n.Rate >= rate).Select(n => n.Word).ToList();
            CreateDic();
            if (LearnEnd != null) LearnEnd(this, new EventArgs());
        }

        /// <summary>
        /// 入力テキストを、分かち書きをして、Ngramで組み合わせを作って、指定割合以上のものを採用する。
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="rate"></param>
        public void LearningParallel(IEnumerable<string> lines, double rate)
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

            var countList = list.GroupBy(n => n).Select(n => new { Word = n.Key, Rate = n.Count() / all });
            learnWordList = countList.Where(n => n.Rate >= rate).Select(n => n.Word).ToList();
            CreateDic();
            if (LearnEnd != null) LearnEnd(this, new EventArgs());
        }


        public Func<string, PreSegment> PreSegmentFunc { get; set; }

        public IEnumerable<string> SegmentExted(string input)
        {
            if (string.IsNullOrEmpty(input))
                return new string[] { };
            List<string> result = new List<string>();

            if (PreSegmentFunc != null)
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
            while (input.Length > postion + 1)
            {
                var i = postion + 4;
                WordDicData worddicdata = WordDicData.GetEmpty();
                if (postion + 2 < input.Length)
                {
                    worddicdata = SearchDic(input.Substring(postion, 2), input.Substring(postion));
                }
                if (worddicdata.IsEmpty() == false)
                {
                    var w = worddicdata.GetWord();
                    if (w.Length > 0) result.Add(w);
                    postion += worddicdata.Length;
                    i = postion + 4;
                    word = seg[i - 1];
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

        public PreSegment(string text, IEnumerable<string> list)
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
            foreach (System.Text.RegularExpressions.Match item in r.Matches(this.Text))
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
