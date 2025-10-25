using System.Collections.Generic;
using BepInEx.Logging;
using UnityEngine;

namespace cs.HoLMod.TaskCheat
{
    internal class TaskList
    {
        //任务文本
        public static List<List<string>> Text_AllTaskOrder = new List<List<string>>
        {
            // 每个数组的第一个元素结构为中文的任务|中文的任务描述，第二个元素为英文的任务|英文的任务描述
            new List<string>
            {
                "【任务】为城中路人代写书信^次|寻找城中街道来往行人中头顶【求信】标识者，为其代写书信赚取钱财。",
                "[Quest] Write ^ letters for pedestrians.|Look for pedestrians in the streets with a [Letter] mark over their heads. They'd pay you to write letters for them."
            },
            new List<string>
            {
                "【任务】接受城中商铺招工并完成^次|城中商铺时有【招工】需求跳出，可安排满足需求的家族成员前往做短工贴补家用。",
                "[Quest] Apply and get shop jobs ^ times.|Occasionally, shops put up [Hiring] signs. Send a clan member to earn some quick cash."
            },
            new List<string>
            {
                "【任务】在城中【粮肉铺】购买【粮食】^袋|鼠标悬置商铺之上可跳出建筑提示，有助于您寻找【粮肉铺】购买粮食，粮食是生活必需品，需保持充足哦。",
                "[Quest] Buy ^ bags of [Grains] from the [Grocery Store].|Hover your mouse cursor over a shop to learn more about it. Locate the [Grocery Store] and buy some grains, which you literally can't live without."
            },
            new List<string>
            {
                "【任务】在城中当铺典当物品^次|鼠标悬置商铺之上可跳出建筑提示，有助于您寻找【当铺】典当物品，换取钱财。",
                "[Quest] Pawn items ^ time at the Pawn Store.|Hover your mouse cursor over a shop to learn more about it. Locate the pawnshop and turn things you don't need into money."
            },
            new List<string>
            {
                "【任务】在城中与街头商贩交易^次|寻找城中街道来往行人中头顶【货物】或【铜钱】标识者，与之交易物品，赚取差价。",
                "[Quest] Trade ^ times with street peddlers.|Check pedestrians with a [Goods] or [Coins] mark. They want to trade, and you might actually make a profit."
            },
            new List<string>
            {
                "【任务】在城中招募^位门客|可点击左上方<color=#31211Dff>【人员】</color>打开本城人员列表中或在城中街道来往行人中寻找合适的【寒门子弟】(鼠标悬置在行人上有助于寻找寒门子弟)，并向其发起招为门客的请求。",
                "[Quest] Recruit ^ retainer in town|Comb through pedestrians to find [civilians] (hover mouse cursor over them to see details) and recruit as retainers."
            },
            new List<string>
            {
                "【任务】在城中钱庄借贷钱财^次|鼠标悬置商铺之上可跳出建筑提示，有助于您寻找【钱庄】借贷，到期时需本息一并还清。",
                "[Quest] Take ^ loan from the local bank.|Hover your mouse cursor over a bank to learn more about it. Locate the [Bank] and take a loan. Don't forget to pay your interest."
            },
            new List<string>
            {
                "【任务】在城中建造属于您家族的^间商铺|在郡城场景状态下，点击下方【建造】按钮打开面板，在【商铺(您家族)】一栏选择可建造的商铺，商铺可以带来持续的收益。",
                "[Quest] Build ^ shop for your own clan.|Click the [Build] button in the city view and select available shops under the [Shops (clan)] tab. They will be your cash cows."
            },
            new List<string>
            {
                "【任务】升级您家族的商铺^次|在城中点击您建造好的商铺，打开【商铺面板】，可以找到升级商铺的功能，点击升级。",
                "[Quest] Upgrade your shops ^ times.|Click on your built shops and click 'Upgrade' in the details panel."
            },
            new List<string>
            {
                "【任务】在您商铺中招募^个伙计|在城中点击您建造好的商铺，打开【商铺面板】，可以找到招募伙计的功能，用合适的薪酬招募伙计可以增加商铺收益。",
                "[Quest] Hire ^ employees for your shops.|Click on a shop you built to bring up the [Shop] panel, where there's a Hire button. An employee with a reasonable pay could increase your shop's profit margin."
            },
            new List<string>
            {
                "【任务】在您祖宅中清除^块杂草|点击下方【地图】按钮打开江山图，找到并进入家族府邸，点击下方【建造】按钮进入编辑模式，点击某祖宅中的某块杂草，点击“X”按钮拆除此建筑，或者按住快速删除【快捷键】点击删除！",
                "[Quest] Demolish ^ patch of weeds in your old estate.|Find your estate in [Map] and enter [Build] mode. Select a patch of weeds, and click [X] or hold the fast delete [Shortcut] to demolish!"
            },
            new List<string>
            {
                "【任务】在您祖宅中移动1个废弃建筑^格|点击下方【地图】按钮打开江山图，找到并进入家族府邸，点击下方【建造】按钮进入编辑模式，鼠标拖动某个废弃建筑即可！",
                "[Quest] Move an abandoned building ^ tiles within your estate.|Find your estate in [Map], enter [Build] mode, and drag an abandoned structure!"
            },
            new List<string>
            {
                "【任务】在您的府邸中修复^间库房|在府邸场景状态下，点击下方【建造】按钮进入【编辑模式】，点击要修复建筑后出现【修复】按钮，点击【修复】按钮修复此建筑。",
                "[Quest] Repair ^ Warehouse in your estate.|In estate view, Click the [Build] button below to enter edit mode. Click on a building you want to repair, click the [Repair] button to repair it."
            },
            new List<string>
            {
                "【任务】在您的府邸中修复^间正房|在府邸场景状态下，点击下方【建造】按钮进入【编辑模式】，点击要修复建筑后出现【修复】按钮，点击【修复】按钮修复此建筑。",
                "[Quest] Repair ^ Main Suite in your estate.|In estate view, Click the [Build] button below to enter edit mode. Click on a building you want to repair, click the [Repair] button to repair it."
            },
            new List<string>
            {
                "【任务】将^名家族成员安排进正房|在府邸中点击您建造好的正房，打开【正房面板】，可以找到安排成员居住的功能。",
                "[Quest] Move ^ clan member into main suites.|Click on a Main Suite you built to bring up the [Main Suite] panel, where you can choose clan members to move in."
            },
            new List<string>
            {
                "【任务】为家族成员迎娶或招赘^位寒门子弟|可点击左上方<color=#31211Dff>【人员】</color>打开本城人员列表中或在城中街道来往行人中寻找合适的寒门子弟(鼠标悬置在行人上有助于寻找寒门子弟)，并向其发起提亲或者招赘。",
                "[Quest] Find ^ civilian bride for your clan.|Roam the streets and find appropriate girls or boys of civilian background (hover mouse over them to see background) and propose marriage."
            },
            new List<string>
            {
                "【任务】在府邸建造^个围墙|进入家族府邸，点击下方【建造】按钮打开面板，在【装饰】一栏选择围墙建筑建造，围墙可增加安全值，防止发生府邸被盗事件。",
                "[Quest] Build ^ walls in the estate.|Click the [Build] button below to open the panel, select walls from the [Decoration] section."
            },
            new List<string>
            {
                "【任务】安排^名族人自动购买粮食|点击下方【族人】按钮打开族人面板，点击一名成年族人打开信息面板，点击下方【族人职责】按钮安排职责。",
                "[Quest] Assign ^ clan member to purchase grain automatically.|Click [Lineage] below, select an adult member, then tap [Set Duty] to set their responsibilities."
            },
            new List<string>
            {
                "【任务】在城中购买一座民居,并安排^位门客|在城中寻找并买下一座中意的民居（多找一会，有便宜的哦），选择家族门客安排进此民居，可自动做工交易赚取铜钱或者其他任务等。",
                "[Quest] Buy a house in town and assign ^ retainer to it|Buy any house and assign retainers to it. Retainers automatically perform assigned functions."
            },
            new List<string>
            {
                "【任务】把时间进行速度调为3倍速|在下方中间显示世间的旁边，点击选择时间进行速度的“3x”选项，时间会加速进行，拥有更好的游戏体验！",
                "[Quest] Set time speed to 3x.|Click the [3x] option next to the time display in the middle below, and the gameplay will speed up."
            },
            new List<string>
            {
                "【任务】为族人纳^名妾室（侧室）|可点击左上方<color=#31211Dff>【人员】</color>打开本城人员列表中或在城中街道来往行人中寻找合适的寒门子弟或者前往其他交好世家选择合适的人选，将其纳为某族人的妾室（侧室）。",
                "[Quest] Expand clan marriages for ^ member.|Select suitable candidates from civilians or allied clan in the county, then arrange them as secondary spouses (concubines) for clan members."
            },
            new List<string>
            {
                "【任务】安排^名族人自动收取商铺利润|点击下方【族人】按钮打开族人面板，点击一名成年族人打开信息面板，点击下方【族人职责】按钮安排职责。",
                "[Quest] Assign ^ clan member to auto-collect profits.|Click [Lineage] below, select an adult member, then tap [Set Duty] to set their responsibilities."
            },
            new List<string>
            {
                "【任务】为您家族购买^个农庄|点击下方【地图】按钮打开江山图，寻找正在出售的【农庄】，经营农庄可养活更多族人。",
                "[Quest] Buy ^ farm for your clan.|Click the [Map] button below to check the map and find [farms] for sale. This is how you create a steady cash flow."
            },
            new List<string>
            {
                "【任务】在您家族农庄中升级农房^次|点击下方【地图】按钮打开江山图，找到并进入您刚刚买下的农庄，在农庄中找到某间【农房】点击打开农房面板，升级此农房，升级农房可提高农庄容纳农户的数量。",
                "[Quest] Upgrade your farm cottages ^ times.|Click on a cottage in farm view, then the 'Upgrade' button in the details panel. This increases their capacity."
            },
            new List<string>
            {
                "【任务】在您家族农庄中开垦^个地块|在农庄场景状态下，点击下方【建造】按钮打开面板，在【生产】一栏选择水田、旱田、或菜园建造皆可。",
                "[Quest] Develop ^ plots of land in your farms.|In farm view, click the [Build] button below and select rice fields, paddy fields, or vegetable gardens under the [Productive] tab."
            },
            new List<string>
            {
                "【任务】在您农庄中建造^间畜舍或作坊|在农庄场景状态下，点击下方【建造】按钮打开面板，在【生产】一栏选择禽舍、羊圈、猪圈、牛棚或作坊用于建造。",
                "[Quest] Build ^ animal pens or workshop in your farms.|In farm view, click the [Build] button below and select animal pens or workshop under the [Productive] tab."
            },
            new List<string>
            {
                "【任务】在您家族农庄中收割^次农作物|在农庄场景中，点击收获田地中产出的粮食、蔬菜、水果等。",
                "[Quest] Harvest ^ times in your farms.|Simply click on grains, vegetables, and fruits ripe for harvest in farm view."
            },
            new List<string>
            {
                "【任务】家族诞生^名新成员|家族成员娶亲后，妻子会有概率怀有身孕，怀胎十月后新成员将诞生。",
                "[Quest] Give birth to ^ baby for your clan.|Once a clan member finds a bride, sooner or later she'd get pregnant. Give 10 months and you get a baby!"
            },
            new List<string>
            {
                "【任务】为年幼家族成员安排先生^位|在家族的空闲门客中，选择合适的人选安排给家族未成年子弟充当私人先生。",
                "[Quest] Hire ^ teacher for your clan's young ones.|Appoint retainers as private tutors for your clan's young."
            },
            new List<string>
            {
                "【任务】为家族成员购买^本书籍，并培养其进行研习|在城中找到一间【书肆】，购买您想要的书籍，然后将其赠与家族成员参透<color=#31211Dff>(注意不要打开前往藏书阁自动借阅，要手动【添加】书籍)</color>，有助于该成员属性增长。",
                "[Quest] Buy ^ book and make sure your clan members master them.|Find a [book store] in the city, buy books and give to clan members to study. Mastering a book boost the member's stats."
            },
            new List<string>
            {
                "【任务】与其他世家的子弟交互^次|在城中或者在其他世家中寻得某位成员，安排您家族成员与其进行交互。",
                "[Quest] Interact ^ times with members of other clans.|Get to know them, in either the streets or their homes. See how you can hang out with them."
            },
            new List<string>
            {
                "【任务】赠送其他世家子弟^次物品|选择一位要交好的世家子弟，点击【投其所好】，赠送他喜欢的物品，物品可在杂货铺、书店、珠宝店等铺子买到。",
                "[Quest] Send ^ gifts to other clan members.|Select a clan member you wish to befriend, click [Flatter] to present them with preferred items, which can be purchased from General stores, Book store, or Jewelry store."
            },
            new List<string>
            {
                "【任务】在地图中解锁^一个郡城|点击下方【地图】按钮打开江山图，点击某个未解锁的郡县区域，解锁本区域。",
                "[Quest] Unlock ^ city on the map.|Click on the [Map] button to bring up the world map, then click on a locked province to unlock."
            },
            new List<string>
            {
                "【任务】前往其他世家拜访^次|点击下方【地图】按钮打开江山图，找到并点击某个世家图标，打开世家面板，安排家族成员拜访！",
                "[Quest] Visit the other clan ^ time.|Click the [Map] button, find a clan icon, and arrange for clan members to visit!"
            },
            new List<string>
            {
                "【任务】与^位其他世家子弟好感度达到30|通过交互可以增加其他世家子弟对您家族的好感度。",
                "[Quest] Relationship with ^ member of other clan reaches 30.|Keep interacting with these people to build up your relationship."
            },
            new List<string>
            {
                "【任务】安排家族成员参加^次科举|每隔一段时间，会有科举事件跳出，届时您可以安排家族成员赴考。",
                "[Quest] Attend ^ National Exam.|The empire regularly hosts National Exams to select talents for government positions. Send your clan's best scholars to try your luck."
            },
            new List<string>
            {
                "【任务】安排^名家族子弟出征从军|每隔一段时间朝廷会出征平叛，安排家族子弟出征从军，从军有机会立下军功，获得官职，但也有战死的风险。",
                "[Quest] Send ^ clan member to the army.|The Imperial Court periodically launches military campaigns to suppress rebellions. Send your gallant young men to start a career in the army. Of course, they could be killed in the battle."
            },
            new List<string>
            {
                "【任务】在您府邸中修复或者建造^间厢房|在府邸场景状态下，新建或修复现有厢房，增加厢房可以招募和容纳更多仆人。",
                "[Quest] Repair​ or build ^ wing in your estate.|In estate view, build new or repair existing wings. These are living quarters for your servants."
            },
            new List<string>
            {
                "【任务】在您的府邸中升级^次厢房|在府邸场景状态下，点击一间厢房打开操作面板，点击升级，升级厢房可增加容纳仆人的数量。",
                "[Quest] Upgrade ^ wing in your estate.|In your estate, select a wing, open its panel, and tap [Upgrade] to increase servant capacity."
            },
            new List<string>
            {
                "【任务】为家族府邸招募^个仆人|在府邸中点击您建造好的厢房，打开【厢房面板】，可以找到招募仆人的功能。",
                "[Quest] Hire ^ servants for your estate.|Click on a wing you built to bring up the [wing] panel, where you can hire servants."
            },
            new List<string>
            {
                "【任务】为府邸中的正房安排^个仆人|在府邸中点击您建造好的正房，打开【正房面板】，可以找到增加仆人的功能。",
                "[Quest] Assign ^ servants to your main suites.|Click on a main suite you built to bring up the [main suite] panel, where you can assign servants to it."
            },
            new List<string>
            {
                "【任务】在府邸中修复或建造^间马厩|在府邸场景状态下，新建或修复现有马厩，马厩可以增加家族可容纳马匹数量。",
                "[Quest] Build ^ barn for your estate.|In estate view, build new or repair existing barns. Your clan will need more and bigger barns as you get more horses."
            },
            new List<string>
            {
                "【任务】买^匹马并赠与您家族成员|在城中找到一间【骡马市】，购买您看中的马匹，然后将其赠与家族成员，可增加该成员武力值，也是该成员游历和游学的必备条件。",
                "[Quest] Buy ^ horse and give them to clan members.|Find a [horse market] in the city, buy horses and give them to clan members. This will increase their Might. Also, they can't go out to see the world, or just for a study tour, without a horse."
            },
            new List<string>
            {
                "【任务】在您府邸中建造^座私塾或演武场|在府邸场景状态下，点击下方【建造】按钮打开面板，在【功能】一栏选择私塾或者演武场建造，私塾和演武场可安排先生为家族子弟授课。",
                "[Quest] Build ^ private school or training grounds in your estate.|In estate view, click [Build] and find private schools or training grounds under the [Functional] tab. These are where your teachers give lessons."
            },
            new List<string>
            {
                "【任务】安排家族成员在城中茶肆喝^次茶|在城中找到一间【茶肆】，安排家族成员任选一种茶座喝茶。",
                "[Quest] Send clan members to tea houses for ^ time.|Find a [tea house] in the city and send a clan member there."
            },
            new List<string>
            {
                "【任务】安排家族成员在城中松竹馆听^次小曲|在城中找到一间【松竹馆】，安排家族成员选一个您喜欢的女子听一曲。",
                "[Quest] Send clan members to courtesan lounges for ^ time.|Find a [courtesan lounge] in the city and send a clan member there. Choose any entertainer and enjoy the show."
            },
            new List<string>
            {
                "【任务】在城中将您拥有的^所民居出租|在城中点击您已经购买的一所还未使用的民居，打开操作面板后，选择出租。",
                "[Quest] Rent out ^ owned house in the city.|Click an owned vacant house in the city, open the action panel, then select [Lease] to lease it."
            },
            new List<string>
            {
                "【任务】安排^名族人自动收取民居租金|点击下方【族人】按钮打开族人面板，点击一名成年族人打开信息面板，点击下方【族人职责】按钮安排职责。",
                "[Quest] Assign ^ clan member to collect rent automatically.|Click [Lineage] below, select an adult member, then tap [Set Duty] to set their responsibilities."
            },
            new List<string>
            {
                "【任务】安排家族成员参与^次比武或比文招亲|在城中来往行人中，会有其他世家成员带着比武或者比文招亲的消息，安排家族子弟参加。",
                "[Quest] Send clan members to ^ dame contest.|Clans want the most outstanding husbands for their daughters, and often they do it in a special form of contest. Send your brilliant young men if there's such a contest going on."
            },
            new List<string>
            {
                "【任务】在城中铁匠铺买^把兵器并赠予家族成员|在城中找到一间【铁匠铺】，购买您满意的兵器，然后将其赠与家族成员，可增加该成员武力值。",
                "[Quest] Buy ^ weapon from the local smithy and give to clan members.|Find a [smithy] in the city and buy any weapons you fancy. Give them to clan members to boost their Might."
            },
            new List<string>
            {
                "【任务】在城中珠宝行买^件珠宝并赠予家族成员|在城中找到一间【珠宝行】，购买您满意的珠宝，然后将其赠与家族成员，可增加家族成员魅力值。",
                "[Quest] Buy ^ jewelry piece from the local jeweler and give to clan members.|Find a [jeweler] in the city and buy any jewelry you fancy. Give them to clan members to boost their Charisma."
            },
            new List<string>
            {
                "【任务】在城中某家商号中完成^次交易|在城中找到一间【商号】，选择一笔您希望进行的交易，并完成它。",
                "[Quest] Do ^ transaction in exchanges.|Find an [exchange] in the city, strike a deal and follow through."
            },
            new List<string>
            {
                "【任务】在未收复城池资助某股叛军^次|点击下方【地图】按钮打开江山图，查看未收复的城池，选择资助此城叛军，资助叛军可增加与其关系值，关系值达到一定程度可向其借兵。",
                "[Quest] Fund rebels in fallen cities ^ time.|Click the [Map] button to bring up the empire map. Choose a fallen city and fund the rebels occupying it. Over time, they may lend a helping hand when you're in need."
            },
            new List<string>
            {
                "【任务】在家族农庄安排^名庄头|进入家族农庄，点击左上角【管理】按钮打开农庄操作面板，点击【安排】按钮，选择作为庄头的合适人选，安排庄头后农庄每年年底可自动交租，无需再手动收获。",
                "[Quest] Appoint ^ manager to your farms.|Enter your farms, click the top-left [Manage] button to open the operations panel, tap [Appoint], select a suitable manager, and they’ll deliver the farm’s annual rent automatically."
            },
            new List<string>
            {
                "【任务】发现并治疗^位病人|查看自家家族成员或者其他世家成员的事件，如发现有身患疾病者为其治疗。",
                "[Quest] Discover and treat ^ patient.|Check the event history of clan members, your own or not. Find those troubled by ailments and heal them."
            },
            new List<string>
            {
                "【任务】为家族府邸新建^所藏书阁|在府邸场景状态下，点击下方【建造】按钮打开面板，在【功能】一栏选择藏书阁建造，藏书阁可以允许族人自动借阅书籍，不用在手动为每位族人添加书籍研读。",
                "[Quest] Build ^ library in your estate.|In estate view, click the [Build] button below to find available structures. Library are under the [Functional] tab."
            },
            new List<string>
            {
                "【任务】向家族府邸的藏书阁放入^本书籍|点击府邸中的一所藏书阁，打开藏书阁面板后，点击【添加】按钮向藏书阁添加书籍。",
                "[Quest] Stock ^ books in the Library|Click a library in your estate, open its panel, and tap [Add] to stock books."
            },
            new List<string>
            {
                "【任务】为^名族人设置自动借阅书籍|点击下方【族人】按钮打开族人面板，点击任意族人打开信息面板，点击【培养】按钮后，选择【研习】一栏，点开自动借阅书籍选项，并选择书籍类型。",
                "[Quest] Enable auto-borrow books for ^ clan member.|Open the [Lineage] panel below, select any member, then go to [Train] → [Read] to enable Auto-Borrow Books and choose a category."
            },
            new List<string>
            {
                "【任务】安排成员参加^次其他家族举办的宴会|与某个世家【关系值】提高之后，会有其他世家成员邀请你参加他们的宴会，安排家族子弟参加，结识更多其他世家子弟。",
                "[Quest] Send delegates to ^ party thrown by other clans.|After improving your ​​Relationship​​ with a clan, members of the clan will invite you to attend their parties. Send over a clan member if you hear about a party going on. Socialize and make friends."
            },
            new List<string>
            {
                "【任务】安排成员参加^次其他家族举办的诗会|与某个世家【关系值】提高之后，会有其他世家成员邀请你参加他们的诗会，安排家族子弟参加，结识更多其他世家子弟。",
                "[Quest] Send delegates to ^ poetry salons thrown by other clans.|After improving your ​​Relationship​​ with a clan, members of the clan will invite you to attend their poetry salons. Send over a clan member if you hear about a party going on. Socialize and make friends."
            },
            new List<string>
            {
                "【任务】在家族的农庄施肥^次|进入家族农庄，点击左上角【管理】按钮打开农庄操作面板，点击【施肥】按钮进入施肥模式，点击地块可施肥增加肥沃度，肥沃度有助于提高农作物的产量。",
                "[Quest] Fertilize your farms ^ times.|Enter your farm, click the top-left [Manage] button to open the operations panel, tap [Fertilize] to enter fertilization mode, then click plots to fertilize."
            },
            new List<string>
            {
                "【任务】发现^次某人被欺辱并为其报官|查看自家家族成员或者其他家族成员的事件，如发现有被人欺辱者，帮其报官。",
                "[Quest] Help the humiliated ^ time.|Check the event history of clan members, your own or not. Help them start a lawsuit if they've been humiliated."
            },
            new List<string>
            {
                "【任务】发现并揭发^次某人的罪行|在城中街道查看来往其他世家成员或者在其他世家查看已结识成员的事件，如有作奸犯科者揭发。",
                "[Quest] Discover and expose ^ crime.|Examine members of other clans, in either the streets or your friends list. Find their dirty secrets and make them pay."
            },
            new List<string>
            {
                "【任务】弹劾其他世家身居要职者^次|查看其他世家有官职成员的事件面板，若发现此人有可弹劾的罪行，可派家族成员弹劾。",
                "[Quest] Impeach members of other clans ^ time.|Clan members holding government positions can petition to impeach their competitors. Kick other clans out of the hierarchy!"
            },
            new List<string>
            {
                "【任务】由您家族率军收复^座城池|点击下方【地图】按钮打开江山图，打开未收复的城池，调集可调军队攻打并成功收复该城池。",
                "[Quest] Lead armies to reclaim ^ city.|Click the [Map] button and send your armies to take back fallen cities."
            },
            new List<string>
            {
                "【任务】任命^位官员|族人出任一个郡城或者县城的主政官员后，在【地图】点击此城打开此城面板，选择空缺官职任命。",
                "[Quest] Appoint ^ official.|Once a clan member governs a county or town, open its panel via ​[Map] and appoint vacant posts."
            },
            new List<string>
            {
                "【任务】罢免^位官员|族人出任一个郡城或者县城的主政官员后，在【地图】点击此城打开此城面板，罢免您不满意的官员。",
                "[Quest] Depose ^ official.|Once a clan member governs a county or town, open its panel via ​[Map] and depose unwanted officials."
            },
            new List<string>
            {
                "【任务】在主政城池新建^座民居|进入郡守是您家族成员的郡城，打开建造面板选择【民房(官府)】一栏的民房进行建造，建造民房可增加民生值。",
                "[Quest] Build ^ new house in your own cities.|If a clan member governs a province, you can build houses in its cities. Just select [House (public)] in the Build panel. Building houses increases local Economy."
            },
            new List<string>
            {
                "【任务】为郡城规划^间商铺|出任郡守后可以自由建造郡城，打开郡城建造面板后，选择【商铺(规划)】一栏，选择要规划建造的商铺后建造，会有其他世家前来竞标。",
                "[Quest] Plan ^ shops for the County City.|Unlock county construction as Governor. Open the build menu, select [Store (public)], then build—competing clans will bid."
            },
            new List<string>
            {
                "【任务】为家族府邸建造^间宴会阁|在府邸场景状态下，点击下方【建造】按钮打开面板，在【功能】一栏选择宴会阁建筑建造。",
                "[Quest] Build ^ party pavilion in your estate.|In estate view, click the [Build] button below to find available structures. Party pavilion are under the [Functional] tab."
            },
            new List<string>
            {
                "【任务】由您家族举办^场宴会|点击府邸中【宴会阁】，打开功能面板，选择举办赏灯宴、赏菊宴或者家宴。",
                "[Quest] Host ^ party.|Click a [party pavilion] in your estate and host parties of any kind - lantern parties, garden parties, or home parties."
            },
            new List<string>
            {
                "【任务】由您家族举办^场诗会|点击府邸中【宴会阁】，打开功能面板，选择举办诗会。",
                "[Quest] Have your clan host ^ poetry salon.|Click a [party pavilion] in your estate and host poetry salons."
            },
            new List<string>
            {
                "【任务】您家族受封封地^次|家族高级武官立下战功时，有机会受封爵位，并获得自己的封地。",
                "[Quest] Get fiefs and titles ^ time.|When achieving a military exploit, your senior military officer may get a fief and title."
            },
            new List<string>
            {
                "【任务】率家族成员在封地猎场狩猎^次|进入家族获得的封地，选择进入一处猎场，可安排家族成员进行狩猎。",
                "[Quest] Participate in ^ hunting trips in your clan fiefs.|Choose a hunting ground in any of your clan fiefs and send clan member to a hunting trip."
            },
            new List<string>
            {
                "【任务】在封地征集民夫修改^次地形|进入家族获得的封地，任选一处地块，征集民夫将其变为您想要的地形（如山地移平）。",
                "[Quest] Draft workers to alter the landscape ^ time.|You are the boss of your own fief. Draft workers to reshape the land to your liking, such as levelling a whole hill."
            },
            new List<string>
            {
                "【任务】为去世成员制作^次牌位并放入祠堂|在府邸中点击您建造或者修复好的祠堂，打开【祠堂面板】，可以选择去世祖先的牌位供奉（去世时制作已牌位）。",
                "[Quest] Make ^ idol for deceased clan members and worship them in the ancestor shrine.|Click any ancestor shrine you've ​repaired​ or built in estate view and put idols you've made there for worship."
            },
            new List<string>
            {
                "【任务】成功奏请^次朝廷出兵某城|点击下方【地图】按钮打开江山图，查看未收复的城池，安排有官职成员奏请朝廷出兵某城。",
                "[Quest] Convince the empire to attack fallen cities ^ time.|Click the [Map] button and find fallen cities on the empire map. Let clan members with government positions to start a petition."
            },
            new List<string>
            {
                "【任务】成功晋见^次皇室成员|族人殿试得中前三或者族中有人京城为官后皇城解锁，进入皇城后，点击皇城建筑跳出的皇室成员图标打开信息面板，点击晋见。",
                "[Quest] Met royals ^.|Unlock the Imperial City when a clan member serves as an official in the capital. Once inside, click the royal icon on buildings to meet the royals."
            },
            new List<string>
            {
                "【任务】为家族府邸建造^座武庙|在府邸场景状态下，点击下方【建造】按钮打开面板，在【功能】一栏选择武庙建筑建造。",
                "[Quest] Build ^ warrior shrine in your estate.|In estate view, click the [Build] button below to find available structures. Warrior shrine are under the [Functional] tab."
            },
            new List<string>
            {
                "【任务】为家族府邸建造^座文庙|在府邸场景状态下，点击下方【建造】按钮打开面板，在【功能】一栏选择文庙建筑建造。",
                "[Quest] Build ^ scholar shrine in your estate.|In estate view, click the [Build] button below to find available structures. Scholar shrine are under the [Functional] tab."
            }
        };

        //任务参数
        public static List<List<string>> AllTaskOrderData = new List<List<string>>
		{
            //{完成任务声望增加值，完成任务所需次数，完成任务铜钱增加值|完成任务元宝增加值}
			new List<string>
			{
				"1",
				"2",
				"100|0"
			},
			new List<string>
			{
				"1",
				"2",
				"200|0"
			},
			new List<string>
            {
                "1",
                "2",
                "200|0"
            },
			new List<string>
            {
                "1",
                "1",
                "300|0"
            },
			new List<string>
            {
                "2",
                "2",
                "300|0"
            },
			new List<string>
            {
                "2",
                "1",
                "400|0"
            },
			new List<string>
            {
                "2",
                "1",
                "400|0"
            },
			new List<string>
            {
                "2",
                "1",
                "500|0"
            },
			new List<string>
            {
                "3",
                "1",
                "500|0"
            },
			new List<string>
            {
                "3",
                "2",
                "600|0"
            },
			new List<string>
            {
                "3",
                "1",
                "600|0"
            },
			new List<string>
            {
                "3",
                "3",
                "700|0"
            },
			new List<string>
            {
                "4",
                "1",
                "700|0"
            },
			new List<string>
            {
                "4",
                "1",
                "800|0"
            },
			new List<string>
            {
                "4",
                "1",
                "800|0"
            },
			new List<string>
            {
                "4",
                "1",
                "1000|0"
            },
			new List<string>
            {
                "5",
                "5",
                "1000|0"
            },
			new List<string>
            {
                "5",
                "1",
                "1200|0"
            },
			new List<string>
            {
                "5",
                "1",
                "1200|0"
            },
			new List<string>
            {
                "5",
                "1",
                "1400|0"
            },
			new List<string>
            {
                "6",
                "1",
                "1400|0"
            },
			new List<string>
            {
                "6",
                "1",
                "1600|0"
            },
			new List<string>
            {
                "6",
                "1",
                "1600|0"
            },
			new List<string>
            {
                "6",
                "2",
                "1800|0"
            },
			new List<string>
            {
                "7",
                "2",
                "1800|0"
            },
			new List<string>
            {
                "7",
                "2",
                "2000|0"
            },
			new List<string>
            {
                "7",
                "4",
                "2000|0"
            },
			new List<string>
            {
                "7",
                "1",
                "2200|0"
            },
			new List<string>
            {
                "8",
                "1",
                "2200|0"
            },
			new List<string>
            {
                "8",
                "1",
                "2400|0"
            },
			new List<string>
            {
                "8",
                "3",
                "2400|0"
            },
			new List<string>
            {
                "8",
                "2",
                "2600|0"
            },
			new List<string>
            {
                "10",
                "1",
                "2600|0"
            },
			new List<string>
            {
                "10",
                "1",
                "2800|0"
            },
			new List<string>
            {
                "10",
                "1",
                "2800|0"
            },
			new List<string>
            {
                "10",
                "1",
                "3000|0"
            },
			new List<string>
            {
                "12",
                "1",
                "3200|0"
            },
			new List<string>
            {
                "12",
                "1",
                "3400|0"
            },
			new List<string>
            {
                "12",
                "1",
                "3600|0"
            },
			new List<string>
            {
                "12",
                "2",
                "3800|0"
            },
			new List<string>
            {
                "14",
                "2",
                "4000|0"
            },
			new List<string>
            {
                "14",
                "1",
                "4200|0"
            },
			new List<string>
            {
                "14",
                "1",
                "4400|0"
            },
			new List<string>
            {
                "14",
                "1",
                "4600|0"
            },
			new List<string>
            {
                "16",
                "1",
                "4800|0"
            },
			new List<string>
            {
                "16",
                "1",
                "5000|0"
            },
			new List<string>
            {
                "16",
                "1",
                "5200|0"
            },
			new List<string>
            {
                "16",
                "1",
                "5400|0"
            },
			new List<string>
            {
                "18",
                "1",
                "5600|0"
            },
			new List<string>
            {
                "18",
                "1",
                "5800|0"
            },
			new List<string>
            {
                "18",
                "1",
                "6000|0"
            },
			new List<string>
            {
                "18",
                "1",
                "6200|0"
            },
			new List<string>
            {
                "20",
                "1",
                "6400|0"
            },
			new List<string>
            {
                "20",
                "1",
                "6600|0"
            },
			new List<string>
            {
                "20",
                "1",
                "6800|0"
            },
			new List<string>
            {
                "20",
                "1",
                "7000|0"
            },
			new List<string>
            {
                "22",
                "2",
                "7200|0"
            },
			new List<string>
            {
                "22",
                "1",
                "7400|0"
            },
			new List<string>
            {
                "22",
                "1",
                "7600|0"
            },
			new List<string>
            {
                "22",
                "1",
                "7800|0"
            },
			new List<string>
            {
                "24",
                "2",
                "8000|0"
            },
			new List<string>
            {
                "24",
                "1",
                "8200|0"
            },
			new List<string>
            {
                "24",
                "1",
                "8400|0"
            },
			new List<string>
            {
                "24",
                "1",
                "8600|0"
            },
			new List<string>
            {
                "26",
                "1",
                "8800|0"
            },
			new List<string>
            {
                "26",
                "1",
                "9000|0"
            },
			new List<string>
            {
                "26",
                "1",
                "9200|0"
            },
			new List<string>
            {
                "26",
                "1",
                "9400|0"
            },
			new List<string>
            {
                "28",
                "2",
                "9600|0"
            },
			new List<string>
            {
                "28",
                "1",
                "9800|0"
            },
			new List<string>
            {
                "28",
                "1",
                "10000|0"
            },
			new List<string>
            {
                "28",
                "1",
                "11000|0"
            },
			new List<string>
            {
                "30",
                "1",
                "12000|0"
            },
			new List<string>
            {
                "30",
                "4",
                "13000|0"
            },
			new List<string>
            {
                "30",
                "1",
                "14000|0"
            },
			new List<string>
            {
                "30",
                "1",
                "15000|0"
            },
			new List<string>
            {
                "32",
                "1",
                "16000|0"
            },
			new List<string>
            {
                "34",
                "2",
                "17000|0"
            },
			new List<string>
            {
                "36",
                "1",
                "18000|0"
            },
			new List<string>
            {
                "38",
                "1",
                "20000|0"
            }
		};

        // 从Mainload读取任务数据并进行对比和更新
        public static void UpdateTasksFromMainload()
        {
            try
            {
                // 获取Mainload对象
                var mainload = GameObject.Find("Mainload");
                if (mainload == null)
                {
                    TaskCheat.Log?.LogWarning("TaskCheat: 未找到Mainload对象");
                    return;
                }

                // 读取Mainload.Text_AllTaskOrder
                var textAllTaskOrderField = mainload.GetType().GetField("Text_AllTaskOrder");
                if (textAllTaskOrderField != null)
                {
                    var newTextTasks = textAllTaskOrderField.GetValue(mainload) as List<List<string>>;
                    if (newTextTasks != null && newTextTasks.Count > 0)
                    {
                        CompareAndUpdateTasks(Text_AllTaskOrder, newTextTasks, "Text_AllTaskOrder");
                    }
                }
                else
                {
                    TaskCheat.Log?.LogWarning("TaskCheat: 未找到Mainload.Text_AllTaskOrder字段");
                }

                // 读取Mainload.AllTaskOrderData
                var allTaskOrderDataField = mainload.GetType().GetField("AllTaskOrderData");
                if (allTaskOrderDataField != null)
                {
                    var newTaskData = allTaskOrderDataField.GetValue(mainload) as List<List<string>>;
                    if (newTaskData != null && newTaskData.Count > 0)
                    {
                        CompareAndUpdateTasks(AllTaskOrderData, newTaskData, "AllTaskOrderData");
                    }
                }
                else
                {
                    TaskCheat.Log?.LogWarning("TaskCheat: 未找到Mainload.AllTaskOrderData字段");
                }
            }
            catch (System.Exception ex)
            {
                TaskCheat.Log?.LogError("TaskCheat: 更新任务数据时发生错误: " + ex.Message);
            }
        }

        // 比较并更新任务列表
        private static void CompareAndUpdateTasks(List<List<string>> existingTasks, List<List<string>> newTasks, string taskType)
        {
            int addedCount = 0;

            // 遍历新任务列表
            foreach (var newTask in newTasks)
            {
                bool found = false;
                
                // 检查现有任务列表中是否已存在该任务
                for (int i = 0; i < existingTasks.Count; i++)
                {
                    if (AreTasksEqual(existingTasks[i], newTask))
                    {
                        found = true;
                        break;
                    }
                }

                // 如果任务不存在，则添加
                if (!found)
                {
                    existingTasks.Add(newTask);
                    addedCount++;
                }
            }

            TaskCheat.Log?.LogInfo($"TaskCheat: {taskType} 更新完成 - 添加了 {addedCount} 个新任务");
        }

        // 比较两个任务是否相同
        private static bool AreTasksEqual(List<string> task1, List<string> task2)
        {
            if (task1.Count != task2.Count)
                return false;

            for (int i = 0; i < task1.Count; i++)
            {
                if (task1[i] != task2[i])
                    return false;
            }

            return true;
        }

        // 获取Logger实例
        private static ManualLogSource Logger
        {
            get { return TaskCheat.Log; }
        }

    }
}
