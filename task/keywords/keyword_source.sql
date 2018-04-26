SELECT  d.NAME                                         AS tablename,
       a.NAME                                           as colname
       
FROM   syscolumns a
       INNER JOIN systypes b
              ON a.xtype = b.xusertype
       INNER JOIN sysobjects d
               ON a.id = d.id
                  AND d.xtype = 'U'
                  AND d.NAME <> 'dtproperties'
WHERE  b.NAME IS NOT NULL
AND b.name NOT in ('int','datetime','decimal','bit','numeric','bigint','char','money')
AND a.name <> 'Data_ encryption'
AND d.name NOT  IN
(
'查询',
'景区交客统计按门市',
'客源结构统计',
'邮轮公司订单视图',
'temp',
'temp_king',
'TreeDemo',
'wangtest',
'Temp_Tab_Auction_NormalLine',
'teshu_ship_hangxian',
'Test_CardsUse',
'V_cjhjyl_Client',
'VouchData',
'VouchDataFlag',
'W_Voucher',
'site_adv',
'site_file',
'site_link',
'site_menu',
'site_nav',
'site_news',
'site_role',
'query_settle_data',
'sys_ding_department',
'Sys_Ding_Login',
'sys_ding_to_user',
'sys_ding_user',
'sys_ding_user_detail',
'Sys_Auction_BrightSet',
'Sys_Auction_Cqssc',
'Sys_Auction_Note',
'Sys_Auction_Product',
'Sys_Auction_Theme',
'app_addresslistinfo',
'app_sweepstakes_cardlist',
'app_sweepstakes_numbers',
'app_sweepstakesinfo',
'app_test',
'App_up',
'app_versioninfo',
'cjhjyl_Client',
'cjhjyl_md_cf',
'cjhjyl_Order_list',
'G_Ship_Room_hj',
'G_Ship_RoomType_hj',
'G_Ship_RoomType_yg',
'G_Ship_Sale_Province',
'G_Ship_Sale_Province2015',
'G_Ship_Year_Task',
'goldship_trans',
'LuckDraw',
'LuckDraw_JP',
'LuckDraw_Note',
'sys_temp',
'Sys_Tutechan',
'QRcodeScan',
'Sys_product_standard_Old',
'Sys_Event_Class',
'Sys_City',
'Sys_Upcard_processNotes',
'Sys_Hotel',
'Sys_Shopping'
)
ORDER BY d.NAME asc