import React, { Component } from 'react';
import Standings from './Standings';
import Games from './Games';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
    }


    render() {

        return (
            <div>
            <h1 id="tabelLabel" >NL East Challenge</h1>
            <Standings />
            <Games />
            </div>
        );
    }
}