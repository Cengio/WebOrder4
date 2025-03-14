﻿<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="TimeoutControl.ascx.vb" Inherits="servisWO.TimeoutControl" %>

<%@ Register assembly="DevExpress.Web.v24.2, Version=24.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<%--<script type="text/javascript">
	window.SessionTimeout = (function() {
		var _timeLeft, _popupTimer, _countDownTimer;

		var stopTimers = function() {
			window.clearTimeout(_popupTimer);
			window.clearTimeout(_countDownTimer);
		};

		var updateCountDown = function() {
			var min = Math.floor(_timeLeft / 60);
			var sec = _timeLeft % 60;
			if(sec < 10)
				sec = "0" + sec;

			document.getElementById("CountDownHolder").innerHTML = min + ":" + sec;

			if(_timeLeft > 0) {
				_timeLeft--;
				_countDownTimer = window.setTimeout(updateCountDown, 1000);
			} else  {
				window.location = <%=QuotedTimeOutUrl%>;
			}            
		};

		var showPopup = function() {
			_timeLeft = 60;
			updateCountDown();
			ClientTimeoutPopup.Show();
		};

		var schedulePopup = function() {       
			stopTimers();
			_popupTimer = window.setTimeout(showPopup, <%=PopupShowDelay%>);
		};

		var sendKeepAlive = function() {
			stopTimers();
			ClientTimeoutPopup.Hide();
			ClientKeepAliveHelper.PerformCallback();
		};

		return {
			schedulePopup: schedulePopup,
			sendKeepAlive: sendKeepAlive
		};

	})();    
</script>--%>

<dx:ASPxPopupControl runat="server" ID="TimeoutPopup" ClientInstanceName="ClientTimeoutPopup"
	CloseAction="None" HeaderText="Session Expiring" Modal="True" PopupHorizontalAlign="WindowCenter"
	PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px" ShowFooter="true"
	AllowDragging="true">
	<ContentCollection>
		<dx:PopupControlContentControl ID="PopupControlContentControl_sessionTimeout" runat="server" SupportsDisabledAttribute="True">
			La tua sessione sta per scadere!
			<br />
			<br />
			<span id="CountDownHolder"></span>
			<br />
			<br />
			Clicca OK per continuare la tua sessione.
		</dx:PopupControlContentControl>
	</ContentCollection>
	<FooterTemplate>
		<dx:ASPxButton runat="server" ID="OkSessionButton" Text="OK" AutoPostBack="false">
			<ClientSideEvents Click="SessionTimeout.sendKeepAlive" />
		</dx:ASPxButton>
	</FooterTemplate>
	<FooterStyle>
		<Paddings Padding="5" />
	</FooterStyle>
</dx:ASPxPopupControl>
<dx:ASPxGlobalEvents runat="server" ID="GlobalEventsTimeoutSession">
	<ClientSideEvents ControlsInitialized="SessionTimeout.schedulePopup" />
</dx:ASPxGlobalEvents>
<dx:ASPxCallback runat="server" ID="KeepAliveHelper" ClientInstanceName="ClientKeepAliveHelper">
</dx:ASPxCallback>