import React, { Component, useState } from 'react';
import { Tooltip } from 'reactstrap';

export class Standings extends Component {
    static displayName = Standings.name;

    constructor(props) {
        super(props);
        this.state = { divisionData: [], loading: true };
    }

    componentDidMount() {
        this.populateDivisionData();
        this.interval = setInterval(() => this.populateDivisionData(), 3600000);
    }

    componentWillUnmount() {
        clearInterval(this.interval);
    }

    static renderDivisionTable(divisionData) {
        const Headers = divisionData.headers.map((header, index) =>
            <th key={index} className={header === 'Streak' ? 'streak-col' : ''}>{header}</th>
        );
        const Rows = divisionData.rows.map((row, rowIndex) =>
            <tr key={rowIndex}>{GetRowData(row, rowIndex)}</tr>
        );
        const Footers = divisionData.footers.map((footer, index) =>
            <td key={index} className={divisionData.headers[index] === 'Streak' ? 'streak-col' : ''}>{footer}</td>
        );

        return (
            <div className="table-responsive">
                <table className='table table-striped standings-table' style={{ tableLayout: 'fixed' }} aria-labelledby="tabelLabel">
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
            </div>
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

function TooltipCell({ column, rowIndex, colIndex }) {
    const [tooltipOpen, setTooltipOpen] = useState(false);
    const toggle = () => setTooltipOpen(!tooltipOpen);
    const cellId = `cell-${rowIndex}-${colIndex}`;
    const hasTooltip = column.winsGuess > 0;
    const isStreak = column.team === 'Streak';

    return (
        <td
            id={cellId}
            style={GetColor(column.value)}
            className={isStreak ? 'streak-col' : ''}
        >
            {GetColumnValue(column)}
            {hasTooltip &&
                <Tooltip
                    placement="top"
                    isOpen={tooltipOpen}
                    target={cellId}
                    toggle={toggle}
                    trigger="hover focus click"
                >
                    {column.winsGuess} wins
                </Tooltip>
            }
        </td>
    );
}

function GetRowData(row, rowIndex) {
    const RowData = row.map((column, colIndex) =>
        <TooltipCell key={colIndex} column={column} rowIndex={rowIndex} colIndex={colIndex} />
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
        return column.teamName;
}

export default Standings;