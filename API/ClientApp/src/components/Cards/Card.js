import React from "react";
import "./card.css";

export function Card() {

  return (
    <div className="card">
      <div className="cardTitle">
        <span>01 de Noviembre de 2022</span>
      </div>
      <div className="cardInfo">
        <div className="water">
            <div className="waterContainer">
                <div className="color"></div>
                <span className="text">8:40 - 70ml</span>
            </div>
            <div className="waterContainer">
                <div className="color"></div>
                <span className="text">8:40 - 70ml</span>
            </div>
            <div className="waterContainer">
                <div className="color"></div>
                <span className="text">8:40 - 70ml</span>
            </div>
            <div className="waterContainer">
                <div className="color"></div>
                <span className="text">8:40 - 70ml</span>
            </div>
        </div>
        <div className="cantidadAgua">
            <div className="cantidad">
                <span>445 ml / 400 ml</span>
            </div>
            <div className="estado">
                <span>Buena</span>
            </div>
        </div>
      </div>
    </div>
  );
}