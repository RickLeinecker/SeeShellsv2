import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import HeatmapImg from '../../assets/heatmap.png';
import HeatmapFilter from '../../assets/heatmap-highlight.png';
import HeatmapTooltip from '../../assets/heatmap-tooltip.png';
import HeatmapRotated from '../../assets/heatmap-rotated.png';

const styles = {
    content: {
        height: '100%',
        width: '100%',
        display: 'flex',
        justifyContent: 'flex-start',
        flexDirection: 'column',
        alignItems: 'center',
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
}


class Heatmap extends React.Component {

    render() {
        return(
            <div className={this.props.classes.content}>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    The heatmap displays frequency of events that occurred per day.
                </Typography>
                <img src={HeatmapImg} alt="heatmap" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Users can filter events on the timeline by right clicking and then dragging the mouse over a portion of the heatmap.
                </Typography>
                <img src={HeatmapFilter} alt="heatmap" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Left clicking on a day will bring up a tooltip that displays information on how many shellbags were taken from that day.
                </Typography>
                <img src={HeatmapTooltip} alt="heatmap" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    The window containing the heatmap can be resized, and if dragged out far enough, the heatmap will rotate onto its side.
                </Typography>
                <img src={HeatmapRotated} alt="heatmap" />
            </div>
        );
    }
}

export default withStyles(styles)(Heatmap);