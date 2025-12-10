const SPREADSHEET_ID = '1XDwrk7MuCOlNCTEg56pvsP8t8BpoNzYVBGZ3FfkxoZI';
const SHEET_NAME = '學生成績';
const SHEET_ASSEMBLY = '小組組裝';

/**
 * 處理 GET 請求,顯示確認頁面
 */
function doGet(e) {
  var data = {
    group: e.parameter.group || '',
    name: e.parameter.name || '',
    coins: e.parameter.coins || '0',
    score: e.parameter.score || '0',
    assembly: e.parameter.assembly || ''
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
 * 2. 如果有組裝資料，寫入小組組裝工作表
 */
function submitData(data) {
  try {
    var ss = SpreadsheetApp.openById(SPREADSHEET_ID);
    
    // ---------------------------
    // 1. 寫入學生成績
    // ---------------------------
    var sheet = ss.getSheetByName(SHEET_NAME);
    if (!sheet) {
      return { 
        success: false, 
        message: '找不到「學生成績」工作表!' 
      };
    }
    
    // 依照使用者需求順序: 組別, 姓名, 測驗成績, 金幣, 時間
    sheet.appendRow([
      data.group,
      data.name,
      data.score,
      data.coins,
      new Date()
    ]);
    
    // ---------------------------
    // 2. 寫入組裝資料 (如果有)
    // ---------------------------
    if (data.assembly && data.assembly !== "") {
      var sheetAssembly = ss.getSheetByName(SHEET_ASSEMBLY);
      if (sheetAssembly) {
        // 欄位: 組別, 組裝資料, 時間
        sheetAssembly.appendRow([
          data.group,
          data.assembly,
          new Date()
        ]);
      }
    }
          data.group,
          data.name,      // 也記錄姓名方便對照
          data.assembly,  // 組裝內容字串
          new Date()
        ]);
      }
    }
    
    return { success: true, message: '提交成功!' };
  } catch (error) {
    return { success: false, message: error.toString() };
  }
}
