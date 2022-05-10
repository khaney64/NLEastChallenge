import React, { Component } from 'react';

export class Mlb extends Component {
    static displayName = Mlb.name;

    constructor(props) {
        super(props);
        this.state = { divisionData: [], loading: true };
    }

    componentDidMount() {
        this.populateDivisionData();
    }

    static renderDivisionTable(divisionData) {
        const Headers = divisionData.headers.map((header) =>
            <th>{header}</th>
        );
        const Rows = divisionData.rows.map((row) =>
            <tr> {GetRowData(row)} </tr>
        );
        const Footers = divisionData.footers.map((footer) =>
            <td>{footer}</td>
        );

        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>{Headers}</tr>
                </thead>
                <tbody>
                    {Rows}
                </tbody>
                <tfoot>
                    <tr>{Footers}</tr>
                </tfoot>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Mlb.renderDivisionTable(this.state.divisionData);

        return (
            <div>
                <h1 id="tabelLabel" >NL East Challenge</h1>
                {contents}
            </div>
        );
    }

    async populateDivisionData() {
        const response = await fetch('divisiondata');
        const data = await response.json();
        this.setState({ divisionData: data, loading: false });
    }
}

function GetRowData(row) {
    const RowData = row.map((column) =>
        <td style={GetColor(column.value)}>{GetColumnValue(column)}</td>
    );

    return RowData;
}

function GetColor(value) {
    if (value == 0)
        return { color: 'black' };
    else
        return { color: 'green', "font-weight": 'bold' };
}

function GetColumnValue(column) {
    if (column.team == 'Record')
        return column.record;
    else if (column.team == 'Streak')
        return column.streak;
    else
        return column.team;
}
