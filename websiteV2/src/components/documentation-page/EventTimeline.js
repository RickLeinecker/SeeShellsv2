import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { Typography, Tabs, Tab, Paper } from '@material-ui/core';
import Timeline from './Timeline.js';
import Heatmap from './Heatmap.js';
import EventTable from './EventTable.js';
import TimelineView from '../../assets/timeline-view.png';

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
    tabs: {
        width: '100%',
    },
}

class EventTimeline extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            view: ''
        }

        this.handleChange = this.handleChange.bind(this);
    }

    /*
    *   handleClick(event)
    *   - takes in a react event object on every button click
    *   - redirects the user to the page associated with the button's id
    */
    handleChange(event) {
        if (event.currentTarget.id === "timeline") {
            this.setState({ view: 'timeline' });
        } else if (event.currentTarget.id === "heatmap") {
            this.setState({ view: 'heatmap' });
        } else if (event.currentTarget.id === "table") {
            this.setState({ view: 'table' });
        }
    }
    
render() {
        return(
            <div className={this.props.classes.content}>
                <Typography variant="title" className={this.props.classes.title}>Event Timeline</Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    The Event Timeline view allows users to view ShellBag Events in chronological order with three views that work with each other. Any and all 
                    filters affect all three views on the Event Timeline.
                </Typography>
                <img src={TimelineView} alt="timeline-view" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    There are three main sections to the Event Timeline: The timeline, the heatmap, and the shellbag event table.
                </Typography>
                <Paper>
                    <Tabs
                        onChange={this.handleChange}
                        indicatorColor="primary"
                        textColor="primary"
                        centered={true}
                        className={this.props.classes.tabs}
                    >
                        <Tab label="Timeline" id='timeline' />
                        <Tab label="Heatmap" id='heatmap' />
                        <Tab label="Event Table" id='table' />
                    </Tabs>
                </Paper>
                { this.state.view === 'timeline' &&
                    <Timeline/>
                }
                { this.state.view === 'heatmap' &&
                    <Heatmap/>
                }
                { this.state.view === 'table' &&
                    <EventTable/>
                }
            </div>
            
        )
    }
}

export default withStyles(styles)(EventTimeline);