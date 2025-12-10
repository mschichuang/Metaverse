const SPREADSHEET_ID = '1XDwrk7MuCOlNCTEg56pvsP8t8BpoNzYVBGZ3FfkxoZI';
const SHEET_STUDENT = '學生成績';
const SHEET_ASSEMBLY = '小組組裝';

/**
 * 處理 GET 請求
 * 根據 mode 參數決定顯示哪個提交頁面
 * mode=quiz  → 提交個人成績
 * mode=assembly → 提交小組組裝
 */
function doGet(e) {
  var mode = e.parameter.mode || 'quiz';
  
  if (mode === 'assembly') {
    // 組裝區提交
    var data = {
      group: e.parameter.group || '',
      tierCase: e.parameter.case || '0',
      tierMB: e.parameter.mb || '0',
      tierCPU: e.parameter.cpu || '0',
      tierCooler: e.parameter.cooler || '0',
      tierRAM: e.parameter.ram || '0',
      tierSSD: e.parameter.ssd || '0',
      tierGPU: e.parameter.gpu || '0',
      tierPSU: e.parameter.psu || '0'
    };
    
    var template = HtmlService.createTemplateFromFile('SubmitPage_Assembly');
    template.data = data;
    
    return template.evaluate()
      .setTitle('提交小組組裝')
      .setSandboxMode(HtmlService.SandboxMode.IFRAME)
      .setXFrameOptionsMode(HtmlService.XFrameOptionsMode.ALLOWALL);
  } else {
    // 測驗區提交（預設）
    var data = {
      group: e.parameter.group || '',
      name: e.parameter.name || '',
      score: e.parameter.score || '0',
      coins: e.parameter.coins || '0'
    };
    
    var template = HtmlService.createTemplateFromFile('SubmitPage_Quiz');
    template.data = data;
    
    return template.evaluate()
      .setTitle('提交個人成績')
      .setSandboxMode(HtmlService.SandboxMode.IFRAME)
      .setXFrameOptionsMode(HtmlService.XFrameOptionsMode.ALLOWALL);
  }
}

/**
 * 提交個人成績（測驗區用）
 * 寫入「學生成績」工作表
 * 欄位: 組別, 姓名, 測驗成績, 金幣, 提交時間
 */
function submitQuizData(data) {
  try {
    var ss = SpreadsheetApp.openById(SPREADSHEET_ID);
    var sheet = ss.getSheetByName(SHEET_STUDENT);
    
    if (!sheet) {
      return { success: false, message: '找不到「學生成績」工作表!' };
    }
    
    sheet.appendRow([
      data.group,
      data.name,
      data.score,
      data.coins,
      new Date()
    ]);
    
    return { success: true, message: '個人成績提交成功!' };
  } catch (error) {
    return { success: false, message: error.toString() };
  }
}

/**
 * 提交小組組裝（組裝區用）
 * 寫入「小組組裝」工作表
 * 欄位: 組別, 機殼, 主機板, 中央處理器, 散熱器, 記憶體, 固態硬碟, 顯示卡, 電源供應器, 提交時間
 */
function submitAssemblyData(data) {
  try {
    var ss = SpreadsheetApp.openById(SPREADSHEET_ID);
    var sheet = ss.getSheetByName(SHEET_ASSEMBLY);
    
    if (!sheet) {
      return { success: false, message: '找不到「小組組裝」工作表!' };
    }
    
    sheet.appendRow([
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
    
    return { success: true, message: '小組組裝提交成功!' };
  } catch (error) {
    return { success: false, message: error.toString() };
  }
}
