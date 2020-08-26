using System;
using System.Collections.Generic;
using System.Net.Http;

namespace CancelamentoIdentity.Services
{
    public class RedemetServices
    {

        private readonly HttpClient httpClient = new HttpClient();

        public string RetrieveMeteorologyData(string stationscsv, DateTime from, DateTime to)
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            string begin = from.ToString("yyyyMMdd") + from.Hour.ToString().PadLeft(2, '0');
            string end = to.ToString("yyyyMMdd") + to.Hour.ToString().PadLeft(2, '0');



            var url = String.Format("http://www.redemet.aer.mil.br/api/consulta_automatica/index.php?local={0}&msg=metar&data_ini={1}&data_fim={2}", ConvertIataToIcao(stationscsv), begin, end);


            var responseMessage = httpClient.GetAsync(new Uri(url)).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseAsString = responseMessage.Content.ReadAsStringAsync().Result;

                return responseAsString;
            }
            else
            {
                throw new Exception();
            }
        }

        private string ConvertIataToIcao(string codIata)
        {
            IDictionary<string, string> IcaoIata = new Dictionary<string, string>();
            IcaoIata.Add("AAX", "SBAX");
            IcaoIata.Add("AFL", "SBAT");
            IcaoIata.Add("AIR", "SWRP");
            IcaoIata.Add("AJU", "SBAR");
            IcaoIata.Add("APQ", "SNAL");
            IcaoIata.Add("AQA", "SBAQ");
            IcaoIata.Add("ARU", "SBAU");
            IcaoIata.Add("ATM", "SBHT");
            IcaoIata.Add("AUX", "SWGN");
            IcaoIata.Add("BAT", "SBBT");
            IcaoIata.Add("BAU", "SBBU");
            IcaoIata.Add("BAZ", "SWBC");
            IcaoIata.Add("BEL", "SBBE");
            IcaoIata.Add("BFH", "SBBI");
            IcaoIata.Add("BGX", "SBBG");
            IcaoIata.Add("BJP", "SBBP");
            IcaoIata.Add("BNU", "SSBL");
            IcaoIata.Add("BPS", "SBPS");
            IcaoIata.Add("BRA", "SNBR");
            IcaoIata.Add("BSB", "SBBR");
            IcaoIata.Add("BVB", "SBBV");
            IcaoIata.Add("BVH", "SBVH");
            IcaoIata.Add("BVS", "SNVS");
            // IcaoIata.Add("BYO", "SJDB");
            IcaoIata.Add("BZC", "SBBZ");
            IcaoIata.Add("CAC", "SBCA");
            IcaoIata.Add("CAF", "SWCA");
            IcaoIata.Add("CAU", "SNRU");
            IcaoIata.Add("CAW", "SBCP");
            IcaoIata.Add("CCI", "SSCK");
            IcaoIata.Add("CCM", "SBCM");
            IcaoIata.Add("CCX", "SWKC");
            IcaoIata.Add("CDJ", "SBAA");
            IcaoIata.Add("CFB", "SBCB");
            IcaoIata.Add("CFC", "SBCD");
            IcaoIata.Add("CFO", "SJHG");
            IcaoIata.Add("CGB", "SBCY");
            IcaoIata.Add("CGH", "SBSP");
            IcaoIata.Add("CGR", "SBCG");
            IcaoIata.Add("CIZ", "SWKO");
            IcaoIata.Add("CKS", "SBCJ");
            IcaoIata.Add("CLN", "SBCI");
            IcaoIata.Add("CLV", "SBCN");
            IcaoIata.Add("CMG", "SBCR");
            IcaoIata.Add("CMP", "SNKE");
            IcaoIata.Add("CNF", "SBCF");
            IcaoIata.Add("CPQ", "SDAM");
            IcaoIata.Add("CPV", "SBKG");
            IcaoIata.Add("CRQ", "SBCV");
            IcaoIata.Add("CWB", "SBCT");
            IcaoIata.Add("CXJ", "SBCX");
            IcaoIata.Add("CZS", "SBCZ");
            IcaoIata.Add("DIQ", "SNDV");
            IcaoIata.Add("DMT", "SWDM");
            // IcaoIata.Add("DOU", "SSDO");
            IcaoIata.Add("DTI", "SNDT");
            IcaoIata.Add("ERM", "SSER");
            IcaoIata.Add("ERN", "SWEI");
            IcaoIata.Add("FBA", "SWOB");
            IcaoIata.Add("FBE", "SSFB");
            IcaoIata.Add("FEN", "SBFN");
            IcaoIata.Add("FLB", "SNQG");
            IcaoIata.Add("FLN", "SBFL");
            IcaoIata.Add("FOR", "SBFZ");
            IcaoIata.Add("FRC", "SIMK");
            IcaoIata.Add("GEL", "SBNM");
            IcaoIata.Add("GIG", "SBGL");
            IcaoIata.Add("GNM", "SNGI");
            IcaoIata.Add("GPB", "SBGU");
            IcaoIata.Add("GRP", "SWGI");
            IcaoIata.Add("GRU", "SBGR");
            IcaoIata.Add("GUZ", "SNGA");
            IcaoIata.Add("GVR", "SBGV");
            IcaoIata.Add("GYN", "SBGO");
            IcaoIata.Add("HUW", "SWHT");
            IcaoIata.Add("IGU", "SBFI");
            IcaoIata.Add("IMP", "SBIZ");
            IcaoIata.Add("IOS", "SBIL");
            IcaoIata.Add("IPN", "SBIP");
            IcaoIata.Add("IRZ", "SWTP");
            IcaoIata.Add("ITB", "SBIH");
            IcaoIata.Add("ITP", "SDUN");
            // IcaoIata.Add("IZA", "SDZY");
            IcaoIata.Add("JCB", "SSJA");
            IcaoIata.Add("JDF", "SBJF");
            IcaoIata.Add("JDO", "SBJU");
            IcaoIata.Add("JIA", "SWJN");
            // IcaoIata.Add("JJD", "SSVV");
            IcaoIata.Add("JOI", "SBJV");
            IcaoIata.Add("JPA", "SBJP");
            IcaoIata.Add("JPR", "SWJI");
            IcaoIata.Add("JRN", "SWJU");
            // IcaoIata.Add("JTC", "SJTC");
            IcaoIata.Add("JUA", "SIZX");
            IcaoIata.Add("LAZ", "SBLP");
            IcaoIata.Add("LBR", "SWLB");
            IcaoIata.Add("LDB", "SBLO");
            IcaoIata.Add("LEC", "SBLE");
            IcaoIata.Add("LIP", "SBLN");
            IcaoIata.Add("LVR", "SWFE");
            IcaoIata.Add("MAB", "SBMA");
            IcaoIata.Add("MAO", "SBEG");
            IcaoIata.Add("MBZ", "SWMW");
            IcaoIata.Add("MCP", "SBMQ");
            IcaoIata.Add("MCZ", "SBMO");
            IcaoIata.Add("MEA", "SBME");
            IcaoIata.Add("MGF", "SBMG");
            IcaoIata.Add("MII", "SBML");
            IcaoIata.Add("MNX", "SBMY");
            IcaoIata.Add("MOC", "SBMK");
            IcaoIata.Add("MQH", "SBMC");
            IcaoIata.Add("MVF", "SBMS");
            IcaoIata.Add("MVS", "SNMU");
            IcaoIata.Add("NAT", "SBSG");
            IcaoIata.Add("NVP", "SWNA");
            IcaoIata.Add("NVT", "SBNF");
            IcaoIata.Add("OAL", "SSKW");
            IcaoIata.Add("OIA", "SDOW");
            IcaoIata.Add("OLC", "SDCG");
            IcaoIata.Add("OPS", "SWSI");
            IcaoIata.Add("ORX", "SNOX");
            IcaoIata.Add("PAV", "SBUF");
            IcaoIata.Add("PCS", "SNPC");
            IcaoIata.Add("PET", "SBPK");
            IcaoIata.Add("PFB", "SBPF");
            IcaoIata.Add("PIN", "SWPI");
            IcaoIata.Add("PLL", "SBMN");
            IcaoIata.Add("PLU", "SBBH");
            IcaoIata.Add("PMG", "SBPP");
            IcaoIata.Add("PMW", "SBPJ");
            IcaoIata.Add("PNB", "SBPN");
            IcaoIata.Add("PNZ", "SBPL");
            IcaoIata.Add("POA", "SBPA");
            IcaoIata.Add("POJ", "SNPD");
            IcaoIata.Add("POO", "SBPC");
            IcaoIata.Add("PPB", "SBDN");
            IcaoIata.Add("PPY", "SNZA");
            IcaoIata.Add("PVH", "SBPV");
            IcaoIata.Add("QBX", "SNOB");
            IcaoIata.Add("QCJ", "SDBK");
            IcaoIata.Add("QHP", "SBTA");
            IcaoIata.Add("QNV", "SDNY");
            IcaoIata.Add("QPS", "SBYS");
            IcaoIata.Add("QSC", "SDSC");
            IcaoIata.Add("RAO", "SBRP");
            IcaoIata.Add("RBB", "SWBR");
            IcaoIata.Add("RBR", "SBRB");
            IcaoIata.Add("RDC", "SNDC");
            IcaoIata.Add("REC", "SBRF");
            IcaoIata.Add("RIA", "SBSM");
            // IcaoIata.Add("ROO", "SWRD");
            IcaoIata.Add("RVD", "SWLC");
            IcaoIata.Add("SDU", "SBRJ");
            IcaoIata.Add("SFK", "SNSW");
            IcaoIata.Add("SJK", "SBSJ");
            IcaoIata.Add("SJL", "SBUA");
            IcaoIata.Add("SJP", "SBSR");
            IcaoIata.Add("SLZ", "SBSL");
            IcaoIata.Add("SOD", "SDCO");
            IcaoIata.Add("SQX", "SSOE");
            IcaoIata.Add("SRA", "SSZR");
            IcaoIata.Add("SSA", "SBSV");
            IcaoIata.Add("SSZ", "SBST");
            IcaoIata.Add("STM", "SBSN");
            IcaoIata.Add("STU", "SBSC");
            IcaoIata.Add("STZ", "SWST");
            IcaoIata.Add("SXO", "SWFX");
            IcaoIata.Add("SXX", "SNFX");
            IcaoIata.Add("TBT", "SBTT");
            IcaoIata.Add("TFF", "SBTF");
            IcaoIata.Add("TFL", "SNTO");
            IcaoIata.Add("THE", "SBTE");
            // IcaoIata.Add("TJL", "SSTL");
            IcaoIata.Add("TMT", "SBTB");
            IcaoIata.Add("TOW", "SBTD");
            IcaoIata.Add("TUR", "SBTU");
            IcaoIata.Add("UBA", "SBUR");
            IcaoIata.Add("UBT", "SDUB");
            IcaoIata.Add("UDI", "SBUL");
            IcaoIata.Add("UMU", "SSUM");
            IcaoIata.Add("UNA", "SBTC");
            IcaoIata.Add("URG", "SBUG");
            IcaoIata.Add("VAG", "SBVG");
            IcaoIata.Add("VAL", "SNVB");
            IcaoIata.Add("VCP", "SBKP");
            IcaoIata.Add("VDC", "SBQV");
            IcaoIata.Add("VIX", "SBVT");
            IcaoIata.Add("VLP", "SWVC");
            IcaoIata.Add("XAP", "SBCH");
            IcaoIata.Add("ROO", "SBRD");
            IcaoIata.Add("BPG", "SBBW");
            IcaoIata.Add("JJD", "SBJE");
            IcaoIata.Add("DOU", "SBDO");
            IcaoIata.Add("SMT", "SBSO");
            IcaoIata.Add("JTC", "SBAE");
            IcaoIata.Add("IZA", "SBZM");
            IcaoIata.Add("TJL", "SBTG");
            IcaoIata.Add("ARX", "SBAC");
            IcaoIata.Add("JJG", "SBJA");
            IcaoIata.Add("LAJ", "SBLJ");
            IcaoIata.Add("PTO", "SBPO");
            IcaoIata.Add("BYO", "SBDB");
            IcaoIata.Add("PGZ", "SBPG");
            IcaoIata.Add("MEU", "SBMD");
            IcaoIata.Add("MVD", "SUMU");
            IcaoIata.Add("EZE", "SAEZ");
            IcaoIata.Add("SCL", "SCEL");
            IcaoIata.Add("VVI", "SLVR");
            IcaoIata.Add("PUJ", "MDPC");
            IcaoIata.Add("COR", "SACO");
            IcaoIata.Add("ROS", "SAAR");
            IcaoIata.Add("PBM", "SMJP");
            IcaoIata.Add("AEP", "SABE");
            IcaoIata.Add("MDZ", "SAME");
            IcaoIata.Add("ASU", "SGAS");
            IcaoIata.Add("UIO", "SEQM");

            return IcaoIata[codIata.ToUpper()];
        }

    }
}
