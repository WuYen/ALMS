// 取得所有 tag名稱為script的 element
var scripts = document.getElementsByTagName('script');
// 取得最後一個 (也就是現在執行的這一個 )element
var currentScript = scripts[scripts.length - 1];
// 將data屬性的值取出
var idx = currentScript.getAttribute('idx');
var lessHeight = 15 + idx * 50;   //typeId == "Report" ? 115 : 165;

function UpdateGridHeight() {
    GridView.SetHeight(0);
    var containerHeight = ASPx.GetDocumentHeight();
    if (document.body.scrollHeight > containerHeight)
        containerHeight = document.body.scrollHeight;

    GridView.SetHeight(LeftPane.GetHeight() - lessHeight);
}

ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
    UpdateGridHeight();
});
ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
    UpdateGridHeight();
});