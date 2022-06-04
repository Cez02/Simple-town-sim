# Development Notes

## To-do

### Idea

The main idea of the project is a town simulator. At its basis it needs a town with a day and night cycle. It also needs a local time which can be either sped up or slowed down (this can be achieved by Time.timeScale I think).

It would be fun to perhaps procedurally generate such a town, although that will be pretty hard.

Ideally the town should have roads and cars which will move on these roads. They will move according to basic traffic rules: if traffic lights say green, go, stop otherwise, and if you see a car in front of you then stop.

### Environment

The people should at the basis move between the following buildings: home and work. This will deem the average life of a worker pretty boring, hence ideally I'm also thinking of the people going to supermarkets after work. Perhaps some people not going to work at all. 

Here we see an idea for a basic class Building, from which the classes home and work will inherit.

### Living things

Here there are 
