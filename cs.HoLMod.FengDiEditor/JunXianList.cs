namespace cs.HoLMod.JunXianData
{
    public class JunXianData
    {
        // 郡数组，索引对应郡ID
        public static string[] JunList = new string[]
        {
            "南郡",     // 0
            "三川郡",   // 1
            "蜀郡",     // 2
            "丹阳郡",   // 3
            "陈留郡",   // 4
            "长沙郡",   // 5
            "会稽郡",   // 6
            "广陵郡",   // 7
            "太原郡",   // 8
            "益州郡",   // 9
            "南海郡",   // 10
            "云南郡"    // 11
        };

        // 二维县数组，第一维是郡索引，第二维是县索引
        public static string[][] XianList = new string[][]
        {
            // 南郡 (索引0)
            new string[] { "临沮", "襄樊", "宜城", "麦城", "华容", "郢亭", "江陵", "夷陵" },
            
            // 三川郡 (索引1)
            new string[] { "平阳", "荥阳", "原武", "阳武", "新郑", "宜阳" },
            
            // 蜀郡 (索引2)
            new string[] { "邛崃", "郫县", "什邡", "绵竹", "新都", "成都" },
            
            // 丹阳郡 (索引3)
            new string[] { "秣陵", "江乘", "江宁", "溧阳", "建邺", "永世" },
            
            // 陈留郡 (索引4)
            new string[] { "长垣", "济阳", "成武", "襄邑", "宁陵", "封丘" },
            
            // 长沙郡 (索引5)
            new string[] { "零陵", "益阳", "湘县", "袁州", "庐陵", "衡山", "建宁", "桂阳" },
            
            // 会稽郡 (索引6)
            new string[] { "曲阿", "松江", "山阴", "余暨" },
            
            // 广陵郡 (索引7)
            new string[] { "平安", "射阳", "海陵", "江都" },
            
            // 太原郡 (索引8)
            new string[] { "大陵", "晋阳", "九原", "石城", "阳曲", "魏榆", "孟县", "中都" },
            
            // 益州郡 (索引9)
            new string[] { "连然", "谷昌", "同劳", "昆泽", "滇池", "俞元", "胜休", "南安" },
            
            // 南海郡 (索引10)
            new string[] { "四会", "阳山", "龙川", "揭岭", "罗阳", "善禺" },
            
            // 云南郡 (索引11)
            new string[] { "云平", "叶榆", "永宁", "遂久", "姑复", "蜻陵", "弄栋", "邪龙" }
        };

        // 根据郡索引和县索引获取郡县名称
        public static string GetJunXianName(int junIndex, int xianIndex)
        {
            if (junIndex >= 0 && junIndex < JunList.Length &&
                xianIndex >= 0 && xianIndex < XianList[junIndex].Length)
            {
                return $"{JunList[junIndex]}-{XianList[junIndex][xianIndex]}";
            }
            return "未知郡县";
        }

        // 根据郡索引获取郡名称
        public static string GetJunName(int junIndex)
        {
            if (junIndex >= 0 && junIndex < JunList.Length)
            {
                return JunList[junIndex];
            }
            return "未知郡";
        }

        // 根据郡索引和县索引获取县名称
        public static string GetXianName(int junIndex, int xianIndex)
        {
            if (junIndex >= 0 && junIndex < XianList.Length &&
                xianIndex >= 0 && xianIndex < XianList[junIndex].Length)
            {
                return XianList[junIndex][xianIndex];
            }
            return "未知县";
        }

        // 获取指定郡的县数量
        public static int GetXianCount(int junIndex)
        {
            if (junIndex >= 0 && junIndex < XianList.Length)
            {
                return XianList[junIndex].Length;
            }
            return 0;
        }

        // 获取郡的总数
        public static int GetJunCount()
        {
            return JunList.Length;
        }
    }
}