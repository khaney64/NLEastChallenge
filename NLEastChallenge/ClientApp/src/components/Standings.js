import React, { Component, useState } from 'react';
import { Tooltip } from 'reactstrap';

export class Standings extends Component {
    static displayName = Standings.name;

    constructor(props) {
        super(props);
        this.state = { divisionData: [], loading: true, mode: 'normal', showScoring: false };
    }

    componentDidMount() {
        this.populateDivisionData();
        this.interval = setInterval(() => this.populateDivisionData(), 3600000);
    }

    componentWillUnmount() {
        clearInterval(this.interval);
    }

    static renderDivisionTable(divisionData, mode, showScoring, onModeChange, onToggleScoring) {
        const Headers = divisionData.headers.map((header, index) =>
            <th key={index} className={header === 'Streak' ? 'streak-col' : header === 'Record' ? 'record-col' : ''}>{header}</th>
        );
        const Rows = divisionData.rows.map((row, rowIndex) =>
            <tr key={rowIndex}>{GetRowData(row, rowIndex, mode)}</tr>
        );
        const Footers = divisionData.footers.map((footer, index) => {
            if (index === 0) {
                return (
                    <td key={index} colSpan="3" className="mode-toggle-footer">
                        <ScoringModeToggle
                            mode={mode}
                            showScoring={showScoring}
                            onModeChange={onModeChange}
                            onToggleScoring={onToggleScoring}
                        />
                    </td>
                );
            }

            if (index === 1 || index === 2) {
                return null;
            }

            const header = divisionData.headers[index];
            const cls = header === 'Streak' ? 'streak-col' : header === 'Record' ? 'record-col' : '';
            return <td key={index} className={cls}>{footer}</td>;
        });

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
                {showScoring && <ScoringDetails mode={mode} />}
            </div>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading Standings...</em></p>
            : Standings.renderDivisionTable(
                this.state.divisionData,
                this.state.mode,
                this.state.showScoring,
                this.changeMode,
                this.toggleScoring);

        return (
            <div>
                {contents}
            </div>
        );
    }

    async populateDivisionData() {
        const response = await fetch(`divisiondata?mode=${this.state.mode}`);
        const data = await response.json();
        this.setState({ divisionData: data, loading: false });
    }

    changeMode = (mode) => {
        if (mode === this.state.mode) {
            return;
        }

        this.setState({ mode, loading: true }, () => this.populateDivisionData());
    }

    toggleScoring = () => {
        this.setState((state) => ({ showScoring: !state.showScoring }));
    }
}

function ScoringModeToggle({ mode, showScoring, onModeChange, onToggleScoring }) {
    return (
        <div className="scoring-controls">
            <div className="scoring-mode-toggle" aria-label="Scoring mode">
                <button
                    type="button"
                    className={mode === 'normal' ? 'active' : ''}
                    onClick={() => onModeChange('normal')}
                    aria-label="Normal scoring"
                    title="Normal scoring"
                >
                    <span aria-hidden="true">⚾</span>
                </button>
                <button
                    type="button"
                    className={mode === 'hh' ? 'active' : ''}
                    onClick={() => onModeChange('hh')}
                    aria-label="Horseshoes and hand grenades scoring"
                    title="Horseshoes and hand grenades scoring"
                >
                    <span aria-hidden="true">🧲</span>
                </button>
            </div>
            <button
                type="button"
                className="scoring-help-button"
                onClick={onToggleScoring}
                aria-expanded={showScoring}
                aria-label="Show scoring explanation"
                title="Show scoring explanation"
            >
                ?
            </button>
        </div>
    );
}

function ScoringDetails({ mode }) {
    const lines = mode === 'hh'
        ? [
            'Horseshoes and hand grenades: teams score close-enough points by rank distance, plus one bonus point for each correctly ordered team pair. If players tie, the tie breaker compares each player top pick wins percentage to that same team current winning percentage. Closest absolute difference wins, so being over or under counts the same.',
            'Colors show why a pick scored: green = close-enough rank points, blue = order bonus only, teal = both.'
        ]
        : [
            'Normal: exact rank matches score 5 points for first place through 1 point for last place. If players tie, the tie breaker compares each player top pick wins percentage to that same team current winning percentage. Closest absolute difference wins, so being over or under counts the same.'
        ];

    return (
        <div className="scoring-help-panel">
            {lines.map((line, index) => <div key={index}>{line}</div>)}
        </div>
    );
}

function TooltipCell({ column, rowIndex, colIndex, mode }) {
    const [tooltipOpen, setTooltipOpen] = useState(false);
    const toggle = () => setTooltipOpen(!tooltipOpen);
    const cellId = `cell-${rowIndex}-${colIndex}`;
    const hasTooltip = column.winsGuess > 0;
    const isStreak = column.team === 'Streak';
    const isRecord = column.team === 'Record';
    const isActualTeam = colIndex === 0 && column.record;

    const cls = [
        isStreak ? 'streak-col' : '',
        isRecord ? 'record-col' : '',
        GetScoreClass(column, mode)
    ].filter(Boolean).join(' ');

    return (
        <td
            id={cellId}
            className={cls}
        >
            {GetColumnValue(column)}
            {isActualTeam && <span className="mobile-record">{column.record}</span>}
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

function GetRowData(row, rowIndex, mode) {
    const RowData = row.map((column, colIndex) =>
        <TooltipCell key={colIndex} column={column} rowIndex={rowIndex} colIndex={colIndex} mode={mode} />
    );

    return RowData;
}

function GetScoreClass(column, mode) {
    if (mode !== 'hh') {
        return column.value > 0 ? 'score-normal' : '';
    }

    const hasRankDistance = column.rankDistanceValue > 0;
    const hasPairwiseBonus = column.pairwiseBonusValue > 0;

    if (hasRankDistance && hasPairwiseBonus) {
        return 'score-mixed';
    }

    if (hasRankDistance) {
        return 'score-rank-distance';
    }

    if (hasPairwiseBonus) {
        return 'score-pairwise';
    }

    return '';
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
