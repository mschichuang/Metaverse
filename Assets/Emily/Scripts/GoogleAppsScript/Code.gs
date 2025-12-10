const SPREADSHEET_ID = '1XDwrk7MuCOlNCTEg56pvsP8t8BpoNzYVBGZ3FfkxoZI';
const SHEET_STUDENT = '學生成績';
const SHEET_ASSEMBLY = '小組組裝';

/**
 * 處理 GET 請求,顯示確認頁面
 * URL參數: group, name, score, coins, case, mb, cpu, cooler, ram, ssd, gpu, psu
 */
function doGet(e) {
  var data = {
    group: e.parameter.group || '',
    name: e.parameter.name || '',
    coins: e.parameter.coins || '0',
    score: e.parameter.score || '0',
    // 8個元件等級
    tierCase: e.parameter.case || '0',
    tierMB: e.parameter.mb || '0',
    tierCPU: e.parameter.cpu || '0',
    tierCooler: e.parameter.cooler || '0',
    tierRAM: e.parameter.ram || '0',
    tierSSD: e.parameter.ssd || '0',
    tierGPU: e.parameter.gpu || '0',
    tierPSU: e.parameter.psu || '0'
  };
  
  var template = HtmlService.createTemplateFromFile('ConfirmPage');
  template.data = data;
  
  return template.evaluate()
    .setTitle('提交學習成果')
    .setSandboxMode(HtmlService.SandboxMode.IFRAME)
    .setXFrameOptionsMode(HtmlService.XFrameOptionsMode.ALLOWALL);
}

/**
 * 提交資料到 Google Sheets
 * 1. 寫入學生成績 (組別、姓名、測驗成績、金幣)
 * 2. 寫入小組組裝 (組別、8個元件等級、時間)
 */
function submitData(data) {
  try {
    var ss = SpreadsheetApp.openById(SPREADSHEET_ID);
    
    // ---------------------------
    // 1. 寫入學生成績
    // 欄位: 組別, 姓名, 測驗成績, 金幣
    // ---------------------------
    var sheetStudent = ss.getSheetByName(SHEET_STUDENT);
    if (!sheetStudent) {
      return { 
        success: false, 
        message: '找不到「學生成績」工作表!' 
      };
    }
    
    sheetStudent.appendRow([
      data.group,
      data.name,
      data.score,
      data.coins
    ]);
    
    // ---------------------------
    // 2. 寫入小組組裝
    // 欄位: 組別, 機殼, 主機板, 中央處理器, 散熱器, 記憶體, 固態硬碟, 顯示卡, 電源供應器, 提交時間
    // ---------------------------
    var sheetAssembly = ss.getSheetByName(SHEET_ASSEMBLY);
    if (sheetAssembly) {
      sheetAssembly.appendRow([
        data.group,
        data.tierCase,
        data.tierMB,
        data.tierCPU,
        data.tierCooler,
        data.tierRAM,
        data.tierSSD,
        data.tierGPU,
        data.tierPSU,
        new Date()
      ]);
    }
    
    return { success: true, message: '提交成功!' };
  } catch (error) {
    return { success: false, message: error.toString() };
  }
}
