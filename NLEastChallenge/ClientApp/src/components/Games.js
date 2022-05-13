import React, { Component } from 'react';

export class Games extends Component {
    static displayName = Games.name;

    constructor(props) {
        super(props);
        this.state = { gameData: [], loading: true };
    }

    componentDidMount() {
        this.populateGameData();
    }

    async populateGameData() {
        //console.log('via populateGameData');
        const response = await fetch('gamedata');
        const data = await response.json();

        this.setState({ gameData: data, loading: false });
    }

    static renderGameTable(comp) {

        const gameData = comp.state.gameData;

        const Rows = gameData.map((game, index) =>
            <tr key={ index }>
                <td key={'game' + index }>{getMatchup(game)}</td>
                <td key={'status' + index}>{getStatus(game)}</td>
                <td key={'score' + index}>{getScore(game)}</td>
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
    if (game.status === 'Final' || game.status === 'In Progress' || game.status === 'Game Over')
        return game.awayScore + '-' + game.homeScore + '  ' + game.inning;

    return '';
}

export default Games;