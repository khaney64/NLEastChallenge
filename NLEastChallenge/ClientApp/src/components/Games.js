import React, { Component } from 'react';

export class Games extends Component {
    static displayName = Games.name;

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
        const response = await fetch('gamedata');
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
            <tr key={ index }>
                <td key={'game' + index }>{getMatchup(game)}</td>
                <td key={'status' + index}>{getStatus(game)}</td>
                <td key={'score' + index}><div><b>{getScore(game)}</b></div> {getInningAndOuts(game)}</td>
            </tr>
        );

        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Game</th>
                        <th>Status</th>
                        <th>Score &nbsp;
                            <img src={require('../images/refresh2.png').default} alt="refresh" style={{ height: '20px', width: '20px', borderWidth: '0' }} onClick={() => { comp.populateGameData(); }} />
                        </th>
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
            ? <p><em>Loading Games...</em></p>
            : Games.renderGameTable(this);

        return (
            <div>
                <h2 id="tabelLabel" >Games Today</h2>
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

function getScore(game) {
    if (game.status === 'Final' || game.status === 'In Progress' || game.status === 'Game Over') {
        return game.awayScore + '-' + game.homeScore;
    }

    return '';
}

function getInningAndOuts(game) {
    const inning = getInning(game);
    const outs = getOuts(game);

    if (inning === '')
        return '';

    if (outs === '')
        return (
            <span>{inning}</span>
        );

    return (
        <span>{inning}&nbsp;&nbsp;{outs}</span>
    );
}

function getInning(game) {
    if (game.status === 'Final' || game.status === 'In Progress' || game.status === 'Game Over') {
        return game.inning;
    }

    return '';
}

function getOuts(game) {
    if (game.status === 'In Progress') {
        if (game.outs === 0) {
            return (
                <div>
                    <div class="dotOff"></div><div class="dotOff"></div><div class="dotOff"></div>
                </div>
            );
        }
        if (game.outs === 1) {
            return (
                <div>
                    <div class="dotOn"></div><div class="dotOff"></div><div class="dotOff"></div>
                </div>
        );
        }
        if (game.outs === 2) {
            return (
                <div>
                    <div class="dotOn"></div><div class="dotOn"></div><div class="dotOff"></div>
                </div>
        );
        }
    }

    return '';
}

export default Games;