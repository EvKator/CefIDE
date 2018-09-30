# CefIDE
Convenient interpreter for creating Internet bots, parsers, based on CefSharp
## Basic Information

There are 4 tabs on main window: Code(your script), Browser, Stack(CefPreter variables), Log. All tabs are selective and with variable sizes

CefIDE uses CefPreter - an interpreter for its own scripting language. 

CefPreter constructed as WPF component and has a built-in browser, in the future also a low-level library for sending http requests. For working with page data it uses XPath

  Main advantage of CefPreter - extensibility.
  It's very easy to create own functions: just make a class like "ChangeElementTextFunction", inherit it from Function (CefPreter.Function), compile, save as "Function.dll" in main folder of CefIDE.

  Currently realized functions: Print, AString, ANumber, ToStr, ToNumber, IsGreater, IsLess, AreEqual, Navigate, Click, Enter, GoBack, GoForward, Reload, Wait, WaitForElement, If, While, InnerHTML, CLBCK, Beep

## Example

The bot navigates to the binary option and starts parsing the percentage of people means the rate will rise and prints it. 
If the percentage is greater than 70, it makes a sound and prints the message "PERCENT GREATER THAN 70!!!".That is, it helps at the right time (depending on your conditions) to buy / sell shares.


### GIF:

![](Example.gif)

### Code:

```
Begin
ANumber interval 2000;

Print Navigate 'https://binomo.com/ru/trading';
WaitForElement '//div[@class="trading-data m-unit-bet-opinions-most"]/div/span[1]';

While 1;
Begin
AString percent InnerHTML '//div[@class="trading-data m-unit-bet-opinions-most"]/div/span[1]';

If IsGreater ToNumber percent 70;
Begin
Print 'PERCENT GREATER THAN 70!!!';
Beep;
End

Print percent;
Wait interval;
End
End
```
