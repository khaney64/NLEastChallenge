import React, { Component } from 'react';
import Standings from './Standings';
import Games from './Games';
import UpcomingGames from './UpcomingGames';

export class Home extends Component {
    static displayName = Home.name;

    render() {

        return (
            <div>
            <h1 id="tabelLabel" >NL East Challenge</h1>
            <Standings />
            <Games />
            <UpcomingGames />
            </div>
        );
    }
}