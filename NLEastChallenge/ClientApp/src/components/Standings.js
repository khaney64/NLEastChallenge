import React, { Component } from 'react';

export class Standings extends Component {
    static displayName = Standings.name;

    constructor(props) {
        super(props);
        this.state = { divisionData: [], loading: true };
    }

    componentDidMount() {
        this.populateDivisionData();
        this.interval = setInterval(() => this.populateDivisionData(), 360000);
    }

    componentWillUnmount() {
        clearInterval(this.interval);
    }

    static renderDivisionTable(divisionData) {
        const Headers = divisionData.headers.map((header, index) =>
            <th key={index}>{header}</th>
        );
        const Rows = divisionData.rows.map((row, index) =>
            <tr key={index}>{GetRowData(row)}</tr>
        );
        const Footers = divisionData.footers.map((footer, index) =>
            <td key={index}>{footer}</td>
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
            ? <p><em>Loading Standings...</em></p>
            : Standings.renderDivisionTable(this.state.divisionData);

        return (
            <div>
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
    const RowData = row.map((column, index) =>
        <td key={index} style={GetColor(column.value)}>{GetColumnValue(column)}</td>
    );

    return RowData;
}

function GetColor(value) {
    if (value === 0)
        return { color: 'black' };
    else
        return { color: 'green', "fontWeight": 'bold' };
}

function GetColumnValue(column) {
    if (column.team === 'Record')
        return column.record;
    else if (column.team === 'Streak')
        return column.streak;
    else
        return column.team;
}

export default Standings;