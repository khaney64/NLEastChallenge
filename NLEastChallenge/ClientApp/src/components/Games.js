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

    static renderGameTable(gameData) {
        const Rows = gameData.map((game) =>
            <tr>
                <td>{game.homeTeam}</td>
                <td>{game.awayTeam}</td>
                <td>{GetStatus(game)}</td>
                <td>{GetScore(game)}</td>
            </tr>
        );

        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Home</th>
                        <th>Away</th>
                        <th>Status</th>
                        <th>Score</th>
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
            : Games.renderGameTable(this.state.gameData);

        return (
            <div>
                <h2 id="tabelLabel" >Games Today</h2>
                {contents}
            </div>
        );
    }

    async populateGameData() {
        const response = await fetch('gamedata');
        const data = await response.json();
        this.setState({ gameData: data, loading: false });
    }
}

function GetStatus(game) {
    if (game.status === 'Scheduled')
        return game.gameTime;

    return game.status;
}

function GetScore(game) {
    if (game.status === 'Final' || game.status === 'In Progress' || game.status === 'Game Over')
        return game.homeScore + '-' + game.awayScore;

    return '';
}

function GetColor(value) {
    if (value === 0)
        return { color: 'black' };
    else
        return { color: 'green', "font-weight": 'bold' };
}

function GetColumnValue(column) {
    if (column.team === 'Record')
        return column.record;
    else if (column.team === 'Streak')
        return column.streak;
    else
        return column.team;
}

export default Games;