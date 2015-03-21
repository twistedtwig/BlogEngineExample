##BlogEngineExample
## An open source ASP.NET MVC 5 - EF blog engine

Originally based off of [NBlog][0].  It has been changed to use Entity Framework 6 and a more split architecture so that it can be integrated into other applications cleanly.  

Tags have been added to a blog entry
 
To see NBlog in action, check out [houseofhawkins.com][1], [AiPoker.co.uk][2].

TODO:
----

 - add js prettify
 - Normal user Registration
 - user settings service to pull from config file.. ensure it gets all values on ctor and caches them.
 - Add auto mapper and project to in the repository to remove any hard coding of includes. 
 - Clean up the repository layer to become more generic
 - Google logon
 - Facebook logon
 - Add a published date field
 - add claims


##License
NBlog is open source under the [The MIT License (MIT)](http://www.opensource.org/licenses/mit-license.php)


[0]: https://github.com/ChrisFulstow/NBlog
[1]: http://houseofhawkins.com
[2]: http://aipoker.co.uk
