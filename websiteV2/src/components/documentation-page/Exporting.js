import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { Typography, Paper } from '@material-ui/core';
import ReactPlayer from "react-player";
import ReportMenu from '../../assets/report-menu.png';
import Modules from '../../assets/modules.png';
import ExportButton from '../../assets/export-button.png';
import ModulesOptions from '../../assets/report-menu-arrow.png';
import Print from '../../assets/print.png';

const styles = {
    content: {
        height: '100%',
        width: '100%',
        display: 'flex',
        justifyContent: 'flex-start',
        flexDirection: 'column',
        alignItems: 'center',
        overflow: 'auto',
        borderRadius: '0',
    },
    title: {
        fontSize: '50px',
        fontWeight: 'bold',
        marginTop: '1%',
        alignSelf: 'center',
        color: '#33A1FD',
        textAlign: 'center',
    },
    text: {
        textAlign: 'center',
        padding: '1%',
    },
    video: {
        maxHeight: '360px',
        maxWidth: '640px',
        minHeight: '360px',
        minWidth: '300px',
        height: '50%',
        width: '50%',
        margin: '10px',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        backgroundColor: '#424242',
    },
}

class Exporting extends React.Component {

    render() {
        return (
            <div className={this.props.classes.content}>
                <Typography variant="title" className={this.props.classes.title}>Exporting</Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    The exporting feature allows users to export shellbags in a report format.
                </Typography>
                <img src={ReportMenu} alt="shellbag-export" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>To export analyzed shellbags into a report format:</b>
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    1. From the global menu, select Export > As Report
                </Typography>
                <img src={ExportButton} alt="shellbag-export" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    2. From the report menu, users can select what types of modules they want added to the report.
                </Typography>
                <img src={Modules} alt="shellbag-export" />
                <Paper className={this.props.classes.video}>
                    <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=SD2aOUyG2Og"/>
                </Paper>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>Module Types:</b>
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>Captioned HeatMap:</b> inserts a heatmap with a text editor to the side. The user can then type in that text editor.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>Captioned Histogram:</b> inserts a histogram with a text editor to the side. The user can then type in that text editor.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>Heatmap and Histogram:</b> inserts a side-by-side image of a heatmap and a histogram.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>Header:</b> inserts a textbox that includes a default header. The user can leave in the default text, add to it, or replace it with their own.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>Heatmap:</b> inserts a heatmap.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>Overview:</b> inserts a pie graph with shellbag event types as percentages, the number of shellbags, the number of shellbag events,
                     and the timespan the shellbags are from.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>TextBox:</b> inserts a blank textbox that the user can type in.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>ShellEvent Table:</b> inserts a shellbag event table filled with shellbags.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>Captioned HeatMap:</b> inserts a heatmap with a text editor to the side. The user can then type in that text editor.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>Timeline Histogram:</b> inserts a timeline.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    All of these views can be interacted with from this menu, to only show the specifics that the user wants to display. Events can be pre-filtered before
                     exporting as a report.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    3. Once all desired views are on the report mock-up, the user can change the order of the modules using the arrows next to each module.
                </Typography>
                <img src={ModulesOptions} alt="shellbag-export" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    4. Modules can be deleted by clicking on the X from the same menu.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    5. When finished, select the print button at the bottom of the menu.
                </Typography>
                <img src={Print} alt="shellbag-export" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    6. The report can then be saved as a PDF or XPS, sent directly to printer, or sent to OneNote, where the user can add annotations using the Windows
                     Print Dialog.
                </Typography>
            </div>
        )
    }
}

export default withStyles(styles)(Exporting);