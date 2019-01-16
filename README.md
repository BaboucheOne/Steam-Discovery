![SteamDiscovery Logo](https://previews.dropbox.com/p/thumb/AAQn1ieIauBHY2Iulryy87P49Vvpjo8uSlHJanfK4AjVNh_c9-PM93ldChUIZdbDa-KDtkUnK1Z2zJ1VemXWP9NyAOfjzIQTbis2JFH2EgdFd-5t6vF3umF1E-Kzftpz4yvWGbW0VhVGlC7n_ax3pDMk6dpBzXdxB2ECZI0cbcGNZnL3NAMI6qvkShaG6uVAmPuuelQmHYMe4MkNAnyY0Lr5805Q0IV2bQkIisyt7Dq18A/p.png?size=1600x1200&size_mode=3)

# Steam-Discovery
Steam Discovery enable you to display all your steam's friends.

## What is it ?

With Steam Discovery, you are now able to get all your steam's friends and much more than your own friends!
Also, find in this project a processing file to easly visualise it in a 2D world.

## How to use it:
* Find the file called "configuration.xml" and fill all the field.

> ##### ***To proceed, you must have:***

    * API key from steam at [steam API key form](https://steamcommunity.com/login/home/?goto=%2Fdev%2Fapikey)
    
    * An origin steamID64 at [steam finder](https://steamidfinder.com/)
    
    * A directory destination (Don't write the filename).
      > [If this field is empty, the returned file will be in the main directory] 

* Get the returned file called 'processing_output.xml' and read it with the processing project given.
  > /!\ You also able to parse the file to get your own visualisation /!\
  
* Origin steamID64 must be __public__, or __able to read friends__.

## What represent each field in _configuration.xml_:

* api_key: This key enable you tu get information from steam's servers.
* origin_steamid64: From whome the generation will start.
* max_layer: How deep SteamDiscovery will find friends.
* xml_destination: Where the final file will be place.
