const SPREADSHEET_ID = '1XDwrk7MuCOlNCTEg56pvsP8t8BpoNzYVBGZ3FfkxoZI';
const SHEET_NAME = '學生成績';

/**
 * 處理 GET 請求,顯示確認頁面
 */
function doGet(e) {
  var data = {
    group: e.parameter.group || '',
    name: e.parameter.name || '',
    coins: e.parameter.coins || '0',
    score: e.parameter.score || '0'
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
 * 注意:請先手動在 Google Sheets 建立「學生成績」Sheet,並建立標題行:
 * | 組別 | 姓名 | 總金幣 | 測驗成績 |
 */
function submitData(data) {
  try {
    var sheet = SpreadsheetApp.openById(SPREADSHEET_ID).getSheetByName(SHEET_NAME);
    
    if (!sheet) {
      return { 
        success: false, 
        message: '找不到「學生成績」工作表,請先手動建立並加入標題行!' 
      };
    }
    
    // 直接新增資料行 (假設第1行是標題)
    sheet.appendRow([
      data.group,
      data.name,
      data.coins,
      data.score
    ]);
    
    return { success: true, message: '提交成功!' };
  } catch (error) {
    return { success: false, message: error.toString() };
  }
}
