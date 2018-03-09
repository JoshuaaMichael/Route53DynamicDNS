# Route53DynamicDNS

This project is a simple Windows Service which updates a designated DNS record in Amazon's Route53 to work for <a href="https://www.noip.com/blog/2014/07/11/dynamic-dns-can-use-2/">Dynamic DNS</a>.</br>
</br>
To have a running "production" type version of this service you'll have to know how to compile it into an installer.</br>

However, to just get this one working on your own machine:</br>
<ul>
	<li><a href="https://www.flynsarmy.com/2015/12/setting-up-dynamic-dns-to-your-home-with-route-53/">Create an IAM user</a> in AWS which can talk to the AWS API</br></li>
	<li>Have a record created in Route53 you'd like to update "home.example.com"</br></li>
	<li>Have the record you'd like to update's HostedZoneId. The ID is viewable on Route53 page.</br></li>
	<li>Take a copy of the config.json.sample file, rename it to config.sample, and put in your values and put the file in the same directory as the service's .exe file</br></li>
	<li>Use <a href="http://www.c-sharpcorner.com/UploadFile/8a67c0/create-and-install-windows-service-step-by-step-in-C-Sharp/">Visual Studio tools</a> to install the service</br></li>
</ul>
# TODO

<img src="https://github.com/JoshuaaMichael/Route53DynamicDNS/blob/master/Images/Checkbox.png?raw=true" height="20" align="absmiddle"/>Break hard coded values into a config file</br>
<img src="https://github.com/JoshuaaMichael/Route53DynamicDNS/blob/master/Images/Checkbox.png?raw=true" height="20" align="absmiddle"/>Make a sample config file</br>
<img src="https://github.com/JoshuaaMichael/Route53DynamicDNS/blob/master/Images/Checkbox.png?raw=true" height="20" align="absmiddle"/>Add simple logging to file</br>
<img src="https://github.com/JoshuaaMichael/Route53DynamicDNS/blob/master/Images/Checkbox.png?raw=true" height="20" align="absmiddle"/>Separate service logic from Route53 logic</br>
<img src="https://github.com/JoshuaaMichael/Route53DynamicDNS/blob/master/Images/Unchecked.png?raw=true" height="20" align="absmiddle"/>Implement a small GUI for handling config settings</br>
<img src="https://github.com/JoshuaaMichael/Route53DynamicDNS/blob/master/Images/Unchecked.png?raw=true" height="20" align="absmiddle"/>Create an installer</br>
<img src="https://github.com/JoshuaaMichael/Route53DynamicDNS/blob/master/Images/Unchecked.png?raw=true" height="20" align="absmiddle"/>Better handling of obscure exceptions which could occur</br>