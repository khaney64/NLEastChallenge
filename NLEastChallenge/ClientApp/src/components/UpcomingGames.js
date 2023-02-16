import React, { Component } from 'react';

export class UpcomingGames extends Component {
    static displayName = UpcomingGames.name;

    constructor(props) {
        super(props);
        this.state = { gameData: [], loading: true, refreshInterval: -1 };
    }

    componentDidMount() {
        this.populateGameData();
    }

    componentWillUnmount() {
        clearInterval(this.interval);
    }

    async populateGameData() {
        //console.log('via populateGameData');
        const response = await fetch('gamedata/upcoming');
        const data = await response.json();

        const newInterval = getRefreshInterval(data);

        if (newInterval !== this.state.refreshInterval) {
            if (typeof this.interval !== 'undefined') {
                clearInterval(this.interval);
            }
            this.interval = setInterval(() => this.populateGameData(), newInterval);
        }
        this.setState({ gameData: data, loading: false, refreshInterval: newInterval });
    }

    static renderGameTable(comp) {

        const gameData = comp.state.gameData;

        const Rows = gameData.map((game, index) =>
            <tr key={index}>
                <td key={'game' + index}>{getMatchup(game)}</td>
                <td key={'status' + index}>{getStatus(game)}</td>
            </tr>
        );

        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Game</th>
                        <th>Status</th>
                    </tr>
                </thead>
                <tbody>
                    {Rows}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading Upcoming Games...</em></p>
            : UpcomingGames.renderGameTable(this);

        return (
            <div>
                <h2 id="tabelLabel" >Upcoming Games</h2>
                {contents}
            </div>
        );
    }
}

function getRefreshInterval(gameData) {
    for (const game of gameData)
    {
        if (game.status === "In Progress")
            return 30000;
    }
    return 600000;
}

function getMatchup(game)
{
    return game.awayTeam + ' at ' + game.homeTeam;
}

function getStatus(game) {
    if (game.status === 'Scheduled' || game.status === 'Pre-Game')
        return game.gameTime;

    return game.status;
}

export default UpcomingGames;